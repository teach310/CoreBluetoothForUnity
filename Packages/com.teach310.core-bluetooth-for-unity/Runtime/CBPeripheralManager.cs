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

        public CBPeripheralManager(ICBPeripheralManagerDelegate peripheralDelegate = null)
        {
            _handle = SafeNativePeripheralManagerHandle.Create(this);
            Delegate = peripheralDelegate;
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
