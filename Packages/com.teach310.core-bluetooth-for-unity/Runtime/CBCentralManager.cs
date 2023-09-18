using System;

namespace CoreBluetooth
{
    /// <summary>
    /// An interface that provides updates for the discovery and management of peripheral devices.
    /// https://developer.apple.com/documentation/corebluetooth/cbcentralmanagerdelegate
    /// </summary>
    public interface CBCentralManagerDelegate
    {
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

        CBCentralManagerDelegate _centralManagerDelegate;
        public CBCentralManagerDelegate centralManagerDelegate
        {
            get => _centralManagerDelegate;
            set
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                _centralManagerDelegate = value;
            }
        }

        public CBManagerState state { get; private set; } = CBManagerState.unknown;

        NativeCentralManagerProxy _nativeCentralManagerProxy;

        CBCentralManager() { }

        ~CBCentralManager() => Dispose(false);

        public static CBCentralManager Create(CBCentralManagerDelegate centralManagerDelegate = null)
        {
            var instance = new CBCentralManager();
            instance._handle = SafeNativeCentralManagerHandle.Create(instance);
            instance.centralManagerDelegate = centralManagerDelegate;
            instance._nativeCentralManagerProxy = new NativeCentralManagerProxy(instance._handle);
            return instance;
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

        public bool isScanning
        {
            get
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                return _nativeCentralManagerProxy.IsScanning();
            }
        }

        internal void OnDidUpdateState(CBManagerState state)
        {
            if (_disposed) return;
            this.state = state;
            centralManagerDelegate?.DidUpdateState(this);
        }

        internal void OnDidDiscoverPeripheral(string peripheralId, string peripheralName, int rssi)
        {
            if (_disposed) return;

            if (!_peripherals.TryGetValue(peripheralId, out var peripheral))
            {
                peripheral = new CBPeripheral(peripheralId, peripheralName);
                _peripherals.Add(peripheralId, peripheral);
            }
            centralManagerDelegate?.DidDiscoverPeripheral(this, peripheral, rssi);
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
