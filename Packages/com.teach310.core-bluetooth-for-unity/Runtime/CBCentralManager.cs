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
        void ConnectedPeripheral(CBCentralManager central, CBPeripheral peripheral) { }
        void DisconnectedPeripheral(CBCentralManager central, CBPeripheral peripheral, CBError error) { }
        void FailedToConnect(CBCentralManager central, CBPeripheral peripheral, CBError error) { }
        void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, int rssi) { }
        void UpdatedState(CBCentralManager central);
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

        internal void DidConnect(string peripheralId)
        {
            if (_disposed) return;
            var peripheral = GetPeripheral(peripheralId);
            if (peripheral == null) return;
            Delegate?.ConnectedPeripheral(this, peripheral);
        }

        internal void DidDisconnectPeripheral(string peripheralId, CBError error)
        {
            if (_disposed) return;
            var peripheral = GetPeripheral(peripheralId);
            if (peripheral == null) return;
            Delegate?.DisconnectedPeripheral(this, peripheral, error);
        }

        internal void DidFailToConnect(string peripheralId, CBError error)
        {
            if (_disposed) return;
            var peripheral = GetPeripheral(peripheralId);
            if (peripheral == null) return;
            Delegate?.FailedToConnect(this, peripheral, error);
        }

        internal void DidDiscoverPeripheral(string peripheralId, string peripheralName, int rssi)
        {
            if (_disposed) return;

            if (!_peripherals.TryGetValue(peripheralId, out var peripheral))
            {
                var nativePeriphalProxy = new NativePeripheralProxy(peripheralId, _handle);
                peripheral = new CBPeripheral(peripheralId, peripheralName, nativePeriphalProxy);
                _peripherals.Add(peripheralId, peripheral);
            }
            Delegate?.DiscoveredPeripheral(this, peripheral, rssi);
        }

        internal void DidUpdateState(CBManagerState state)
        {
            if (_disposed) return;
            this.State = state;
            Delegate?.UpdatedState(this);
        }

        internal void PeripheralDidDiscoverServices(string peripheralId, string[] serviceUUIDs, CBError error)
        {
            if (_disposed) return;
            var peripheral = GetPeripheral(peripheralId);
            if (peripheral == null) return;

            var services = serviceUUIDs.Select(uuid => new CBService(uuid, peripheral)).ToArray();
            peripheral.DidDiscoverServices(services, error);
        }

        internal void PeripheralDidDiscoverCharacteristics(string peripheralId, string serviceUUID, string[] characteristicUUIDs, CBError error)
        {
            if (_disposed) return;
            var peripheral = GetPeripheral(peripheralId);
            if (peripheral == null) return;

            var service = peripheral.Services.FirstOrDefault(s => s.UUID == serviceUUID);
            if (service == null)
            {
                UnityEngine.Debug.LogError($"Service {serviceUUID} not found.");
                return;
            }

            var characteristics = characteristicUUIDs.Select(uuid =>
            {
                var nativeCharacteristicProxy = new NativeCharacteristicProxy(peripheralId, serviceUUID, uuid, _handle);
                return new CBCharacteristic(uuid, service, nativeCharacteristicProxy);
            }).ToArray();
            peripheral.DidDiscoverCharacteristics(characteristics, service, error);
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
