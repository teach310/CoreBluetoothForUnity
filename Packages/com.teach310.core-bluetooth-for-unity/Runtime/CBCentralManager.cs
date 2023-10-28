using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CoreBluetooth
{
    /// <summary>
    /// An interface that provides updates for the discovery and management of peripheral devices.
    /// https://developer.apple.com/documentation/corebluetooth/cbcentralmanagerdelegate
    /// </summary>
    public interface ICBCentralManagerDelegate
    {
        void DidConnectPeripheral(CBCentralManager central, CBPeripheral peripheral) { }
        void DidDisconnectPeripheral(CBCentralManager central, CBPeripheral peripheral, CBError error) { }
        void DidFailToConnectPeripheral(CBCentralManager central, CBPeripheral peripheral, CBError error) { }
        void DidDiscoverPeripheral(CBCentralManager central, CBPeripheral peripheral, int rssi) { }
        void DidUpdateState(CBCentralManager central);
    }

    /// <summary>
    /// An object that scans for, discovers, connects to, and manages peripherals.
    /// https://developer.apple.com/documentation/corebluetooth/cbcentralmanager
    /// </summary>
    public class CBCentralManager : CBManager, INativeCentralManagerDelegate, IDisposable
    {
        bool _disposed = false;
        SafeNativeCentralManagerHandle _handle;

        // key: peripheralId
        Dictionary<string, CBPeripheral> _peripherals = new Dictionary<string, CBPeripheral>();

        ICBCentralManagerDelegate _delegate;
        public ICBCentralManagerDelegate Delegate
        {
            get => _delegate;
            set
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                _delegate = value;
            }
        }

        /// <summary>
        /// ICBCentralManagerDelegate callbacks will be called in this context.
        /// </summary>
        public SynchronizationContext CallbackContext { get; set; }

        NativeCentralManagerProxy _nativeCentralManagerProxy;

        public CBCentralManager(ICBCentralManagerDelegate centralDelegate = null, CBCentralManagerInitOptions options = null)
        {
            if (options == null)
            {
                _handle = SafeNativeCentralManagerHandle.Create();
            }
            else
            {
                using var optionsDict = options.ToNativeDictionary();
                _handle = SafeNativeCentralManagerHandle.Create(optionsDict.Handle);
            }
            Delegate = centralDelegate;
            _nativeCentralManagerProxy = new NativeCentralManagerProxy(_handle, this);
            CallbackContext = SynchronizationContext.Current;
        }

        public void Connect(CBPeripheral peripheral)
        {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
            _nativeCentralManagerProxy.Connect(peripheral);
        }

        public void CancelPeripheralConnection(CBPeripheral peripheral)
        {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
            _nativeCentralManagerProxy.CancelPeripheralConnection(peripheral);
        }

        public CBPeripheral[] RetrievePeripherals(params string[] peripheralIds)
        {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
            var peripheralHandles = _nativeCentralManagerProxy.RetrievePeripherals(peripheralIds);
            var result = new CBPeripheral[peripheralHandles.Length];
            for (var i = 0; i < peripheralHandles.Length; i++)
            {
                var peripheral = new CBPeripheral(peripheralHandles[i], CallbackContext);
                SetPeripheral(peripheral);
                result[i] = peripheral;
            }
            return result;
        }

        public void ScanForPeripherals(string[] serviceUUIDs = null)
        {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
            _nativeCentralManagerProxy.ScanForPeripherals(serviceUUIDs);
        }

        public void StopScan()
        {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
            _nativeCentralManagerProxy.StopScan();
        }

        public bool IsScanning
        {
            get
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                return _nativeCentralManagerProxy.IsScanning();
            }
        }

        CBPeripheral GetPeripheral(string peripheralId)
        {
            if (!_peripherals.TryGetValue(peripheralId, out var peripheral))
            {
                UnityEngine.Debug.LogError("Peripheral not found.");
                return null;
            }
            return peripheral;
        }

        void SetPeripheral(CBPeripheral peripheral)
        {
            if (_peripherals.ContainsKey(peripheral.Identifier))
            {
                _peripherals[peripheral.Identifier].Dispose();
            }

            _peripherals[peripheral.Identifier] = peripheral;
        }

        void INativeCentralManagerDelegate.DidConnect(string peripheralId)
        {
            CallbackContext.Post(_ =>
            {
                if (_disposed) return;
                var peripheral = GetPeripheral(peripheralId);
                if (peripheral == null) return;
                Delegate?.DidConnectPeripheral(this, peripheral);
            }, null);
        }

        void INativeCentralManagerDelegate.DidDisconnectPeripheral(string peripheralId, CBError error)
        {
            CallbackContext.Post(_ =>
            {
                if (_disposed) return;
                var peripheral = GetPeripheral(peripheralId);
                if (peripheral == null) return;
                Delegate?.DidDisconnectPeripheral(this, peripheral, error);
            }, null);
        }

        void INativeCentralManagerDelegate.DidFailToConnect(string peripheralId, CBError error)
        {
            CallbackContext.Post(_ =>
            {
                if (_disposed) return;
                var peripheral = GetPeripheral(peripheralId);
                if (peripheral == null) return;
                Delegate?.DidFailToConnectPeripheral(this, peripheral, error);
            }, null);
        }

        void INativeCentralManagerDelegate.DidDiscoverPeripheral(SafeNativePeripheralHandle peripheralHandle, int rssi)
        {
            CallbackContext.Post(_ =>
            {
                if (_disposed) return;
                var peripheral = new CBPeripheral(peripheralHandle, CallbackContext);
                SetPeripheral(peripheral);
                Delegate?.DidDiscoverPeripheral(this, peripheral, rssi);
            }, null);
        }

        void INativeCentralManagerDelegate.DidUpdateState(CBManagerState state)
        {
            CallbackContext.Post(_ =>
            {
                if (_disposed) return;
                this.State = state;
                Delegate?.DidUpdateState(this);
            }, null);
        }

        public void Dispose()
        {
            if (_disposed) return;

            _handle?.Dispose();
            foreach (var peripheral in _peripherals.Values)
            {
                peripheral.Dispose();
            }

            _disposed = true;
        }
    }
}
