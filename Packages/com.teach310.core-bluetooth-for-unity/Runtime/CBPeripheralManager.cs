using System;
using System.Collections.Generic;

namespace CoreBluetooth
{
    public interface ICBPeripheralManagerDelegate
    {
        void DidUpdateState(CBPeripheralManager peripheral);
        void DidAddService(CBPeripheralManager peripheral, CBService service, CBError error) { }
    }

    public class CBPeripheralManager : CBManager, IDisposable
    {
        bool _disposed = false;
        SafeNativePeripheralManagerHandle _handle;
        NativePeripheralManagerProxy _nativePeripheralManagerProxy;

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

        Dictionary<string, CBMutableService> _services = new Dictionary<string, CBMutableService>();
        HashSet<string> _addingServiceUUIDs = new HashSet<string>();

        public CBPeripheralManager(ICBPeripheralManagerDelegate peripheralDelegate = null)
        {
            _handle = SafeNativePeripheralManagerHandle.Create(this);
            Delegate = peripheralDelegate;
            _nativePeripheralManagerProxy = new NativePeripheralManagerProxy(_handle);
        }

        public void AddService(CBMutableService service)
        {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
            if (_services.ContainsKey(service.UUID))
            {
                throw new ArgumentException($"Service {service} is already added.");
            }

            _addingServiceUUIDs.Add(service.UUID);
            _services.Add(service.UUID, service);
            _nativePeripheralManagerProxy.AddService(service);
        }

        public void StartAdvertising(StartAdvertisingOptions options = null)
        {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
            _nativePeripheralManagerProxy.StartAdvertising(options);
        }

        public void StopAdvertising()
        {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
            _nativePeripheralManagerProxy.StopAdvertising();
        }

        public bool IsAdvertising
        {
            get
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                return _nativePeripheralManagerProxy.IsAdvertising;
            }
        }

        internal void DidUpdateState(CBManagerState state)
        {
            if (_disposed) return;
            State = state;
            _delegate?.DidUpdateState(this);
        }

        internal void DidAddService(string serviceUUID, CBError error)
        {
            if (_disposed) return;
            if (!_addingServiceUUIDs.Remove(serviceUUID))
            {
                return;
            }

            _delegate?.DidAddService(this, _services[serviceUUID], error);
        }

        public void Dispose()
        {
            if (_disposed) return;

            _handle?.Dispose();

            _disposed = true;
        }
    }
}
