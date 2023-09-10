using System;

namespace CoreBluetooth
{
    /// <summary>
    /// An interface that provides updates for the discovery and management of peripheral devices.
    /// https://developer.apple.com/documentation/corebluetooth/cbcentralmanagerdelegate
    /// </summary>
    public interface CBCentralManagerDelegate
    {
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

        CBCentralManager() { }

        ~CBCentralManager() => Dispose(false);

        public static CBCentralManager Create(CBCentralManagerDelegate centralManagerDelegate = null)
        {
            var instance = new CBCentralManager();
            instance._handle = SafeNativeCentralManagerHandle.Create(instance);
            instance.centralManagerDelegate = centralManagerDelegate;
            return instance;
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
