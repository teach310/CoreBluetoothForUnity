using System;
using System.Collections.Generic;

namespace CoreBluetooth
{
    public interface ICBPeripheralManagerDelegate
    {
        void DidUpdateState(CBPeripheralManager peripheral);
        void DidAddService(CBPeripheralManager peripheral, CBService service, CBError error) { }
        void DidStartAdvertising(CBPeripheralManager peripheral, CBError error) { }
    }

    internal interface IPeripheralManagerData
    {
        void AddCentral(CBCentral central);
        CBCentral FindCentral(string centralId);
        CBCharacteristic FindCharacteristic(string serviceUUID, string characteristicUUID);
    }

    public class CBPeripheralManager : CBManager, IPeripheralManagerData, IDisposable
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

        // key: centralId
        Dictionary<string, CBCentral> _centrals = new Dictionary<string, CBCentral>();

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

        void IPeripheralManagerData.AddCentral(CBCentral central)
        {
            _centrals.Add(central.Identifier, central);
        }

        CBCentral IPeripheralManagerData.FindCentral(string centralId)
        {
            if (_centrals.TryGetValue(centralId, out var central))
            {
                return central;
            }
            return null;
        }

        CBCharacteristic IPeripheralManagerData.FindCharacteristic(string serviceUUID, string characteristicUUID)
        {
            if (_services.TryGetValue(serviceUUID, out var service))
            {
                return service.FindCharacteristic(characteristicUUID);
            }
            return null;
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

        internal void DidStartAdvertising(CBError error)
        {
            if (_disposed) return;
            _delegate?.DidStartAdvertising(this, error);
        }

        public void Dispose()
        {
            if (_disposed) return;

            _handle?.Dispose();
            foreach (var central in _centrals.Values)
            {
                central.Dispose();
            }

            _disposed = true;
        }
    }
}
