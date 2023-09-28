using System;

namespace CoreBluetooth
{
    public interface ICBPeripheralManagerDelegate
    {
        void DidUpdateState(CBPeripheralManager peripheral);
    }

    public class CBPeripheralManager : CBManager, IDisposable
    {
        bool _disposed = false;
        SafeNativePeripheralManagerHandle _handle;

        ICBPeripheralManagerDelegate _delegate;
        public ICBPeripheralManagerDelegate Delegate
        {
            get => _delegate;
            set
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                _delegate = value;
            }
        }

        CBPeripheralManager() : base() { }

        public static CBPeripheralManager Create(ICBPeripheralManagerDelegate peripheralDelegate = null)
        {
            var instance = new CBPeripheralManager();
            instance._handle = SafeNativePeripheralManagerHandle.Create(instance);
            instance.Delegate = peripheralDelegate;
            return instance;
        }

        internal void DidUpdateState(CBManagerState state)
        {
            State = state;
            _delegate?.DidUpdateState(this);
        }

        public void Dispose()
        {
            if (_disposed) return;

            _handle?.Dispose();

            _disposed = true;
        }
    }
}
