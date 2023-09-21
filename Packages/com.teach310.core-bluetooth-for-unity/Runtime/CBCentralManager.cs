using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreBluetooth
{
    /// <summary>
    /// An interface that provides updates for the discovery and management of peripheral devices.
    /// https://developer.apple.com/documentation/corebluetooth/cbcentralmanagerdelegate
    /// </summary>
    public interface ICBCentralManagerDelegate
    {
        void DidConnect(CBCentralManager central, CBPeripheral peripheral);
        void DidDisconnectPeripheral(CBCentralManager central, CBPeripheral peripheral, CBError error);
        void DidFailToConnect(CBCentralManager central, CBPeripheral peripheral, CBError error);
        void DidDiscoverPeripheral(CBCentralManager central, CBPeripheral peripheral, int rssi);
        void DidUpdateState(CBCentralManager central);
    }

    /// <summary>
    /// An object that scans for, discovers, connects to, and manages peripherals.
    /// https://developer.apple.com/documentation/corebluetooth/cbcentralmanager
    /// </summary>
    public class CBCentralManager : IDisposable
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

        public CBManagerState State { get; private set; } = CBManagerState.Unknown;

        NativeCentralManagerProxy _nativeCentralManagerProxy;

        CBCentralManager() { }

        ~CBCentralManager() => Dispose(false);

        public static CBCentralManager Create(ICBCentralManagerDelegate centralDelegate = null)
        {
            var instance = new CBCentralManager();
            instance._handle = SafeNativeCentralManagerHandle.Create(instance);
            instance.Delegate = centralDelegate;
            instance._nativeCentralManagerProxy = new NativeCentralManagerProxy(instance._handle);
            return instance;
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

        void ThrowIfPeripheralNotDiscovered(CBPeripheral peripheral)
        {
            if (!_peripherals.ContainsKey(peripheral.Identifier))
            {
                throw new ArgumentException($"Peripheral {peripheral} is not discovered.");
            }
        }

        public void Connect(CBPeripheral peripheral)
        {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
            ThrowIfPeripheralNotDiscovered(peripheral);

            _nativeCentralManagerProxy.Connect(peripheral.Identifier);
        }

        public void CancelPeripheralConnection(CBPeripheral peripheral)
        {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
            ThrowIfPeripheralNotDiscovered(peripheral);
            _nativeCentralManagerProxy.CancelPeripheralConnection(peripheral.Identifier);
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

        internal void OnDidConnect(string peripheralId)
        {
            if (_disposed) return;
            var peripheral = GetPeripheral(peripheralId);
            if (peripheral == null) return;
            Delegate?.DidConnect(this, peripheral);
        }

        internal void OnDidDisconnectPeripheral(string peripheralId, CBError error)
        {
            if (_disposed) return;
            var peripheral = GetPeripheral(peripheralId);
            if (peripheral == null) return;
            Delegate?.DidDisconnectPeripheral(this, peripheral, error);
        }

        internal void OnDidFailToConnect(string peripheralId, CBError error)
        {
            if (_disposed) return;
            var peripheral = GetPeripheral(peripheralId);
            if (peripheral == null) return;
            Delegate?.DidFailToConnect(this, peripheral, error);
        }

        internal void OnDidDiscoverPeripheral(string peripheralId, string peripheralName, int rssi)
        {
            if (_disposed) return;

            if (!_peripherals.TryGetValue(peripheralId, out var peripheral))
            {
                var nativePeriphalProxy = new NativePeripheralProxy(peripheralId, _handle);
                peripheral = new CBPeripheral(peripheralId, peripheralName, nativePeriphalProxy);
                _peripherals.Add(peripheralId, peripheral);
            }
            Delegate?.DidDiscoverPeripheral(this, peripheral, rssi);
        }

        internal void OnDidUpdateState(CBManagerState state)
        {
            if (_disposed) return;
            this.State = state;
            Delegate?.DidUpdateState(this);
        }

        internal void OnPeripheralDidDiscoverServices(string peripheralId, string[] serviceUUIDs, CBError error)
        {
            if (_disposed) return;
            var peripheral = GetPeripheral(peripheralId);
            if (peripheral == null) return;

            var services = serviceUUIDs.Select(uuid => new CBService(uuid, peripheral)).ToArray();
            peripheral.OnDidDiscoverServices(services, error);
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (_handle != null && !_handle.IsInvalid)
            {
                _handle.Dispose();
            }

            _disposed = true;
        }
    }
}
