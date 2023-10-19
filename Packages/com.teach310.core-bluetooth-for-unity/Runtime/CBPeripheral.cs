using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CoreBluetooth
{
    public enum CBPeripheralState
    {
        Disconnected = 0,
        Connecting,
        Connected,
        Disconnecting
    }

    public interface ICBPeripheralDelegate
    {
        void DidDiscoverServices(CBPeripheral peripheral, CBError error) { }
        void DidDiscoverCharacteristics(CBPeripheral peripheral, CBService service, CBError error) { }
        void DidUpdateValueForCharacteristic(CBPeripheral peripheral, CBCharacteristic characteristic, CBError error) { }
        void DidWriteValueForCharacteristic(CBPeripheral peripheral, CBCharacteristic characteristic, CBError error) { }
        void DidUpdateNotificationStateForCharacteristic(CBPeripheral peripheral, CBCharacteristic characteristic, CBError error) { }
    }

    /// <summary>
    /// A remote peripheral device.
    /// https://developer.apple.com/documentation/corebluetooth/cbperipheral
    /// </summary>
    public class CBPeripheral : INativePeripheralDelegate, IDisposable
    {
        bool _disposed = false;
        internal SafeNativePeripheralHandle Handle { get; }
        NativePeripheralProxy _nativePeripheral = null;

        string _identifier = null;
        public string Identifier
        {
            get
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);

                if (_identifier == null)
                {
                    _identifier = _nativePeripheral.Identifier;
                }
                return _identifier;
            }
        }

        public string Name
        {
            get
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                return _nativePeripheral.Name;
            }
        }

        public ICBPeripheralDelegate Delegate { get; set; }
        List<CBService> _services = new List<CBService>();
        public ReadOnlyCollection<CBService> Services { get; }

        internal CBPeripheral(SafeNativePeripheralHandle nativePeripheral)
        {
            Handle = nativePeripheral;
            _nativePeripheral = new NativePeripheralProxy(Handle, this);
            this.Services = _services.AsReadOnly();
        }

        /// <summary>
        /// Discovers the specified services of the peripheral.
        /// If the servicesUUIDs parameter is nil, this method returns all of the peripheral’s available services. This is much slower than providing an array of service UUIDs to search for.
        /// </summary>
        public void DiscoverServices(string[] serviceUUIDs = null) {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
            _nativePeripheral.DiscoverServices(serviceUUIDs);
        }

        /// <summary>
        /// Discovers the specified characteristics of a service.
        /// </summary>
        public void DiscoverCharacteristics(string[] characteristicUUIDs, CBService service) {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
            _nativePeripheral.DiscoverCharacteristics(characteristicUUIDs, service);
        }

        /// <summary>
        /// Discover all characteristics in a service (slow).
        /// </summary>
        public void DiscoverCharacteristics(CBService service) {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
            DiscoverCharacteristics(null, service);
        }

        /// <summary>
        /// Retrieves the value of a specified characteristic.
        /// When you call this method to read the value of a characteristic, the peripheral calls the peripheral(_:didUpdateValueFor:error:) method of its delegate object.
        /// If the peripheral successfully reads the value of the characteristic, you can access it through the characteristic’s value property.
        /// </summary>
        public void ReadValue(CBCharacteristic characteristic) {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
            _nativePeripheral.ReadValue(characteristic);
        }

        public void WriteValue(byte[] data, CBCharacteristic characteristic, CBCharacteristicWriteType type)
        {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
            _nativePeripheral.WriteValue(data, characteristic, type);
        }

        /// <summary>
        /// The maximum amount of data, in bytes, you can send to a characteristic in a single write type.
        /// </summary>
        public int GetMaximumWriteValueLength(CBCharacteristicWriteType type)
        {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
            return _nativePeripheral.GetMaximumWriteValueLength(type);
        }

        /// <summary>
        /// Sets notifications or indications for the value of a specified characteristic.
        /// </summary>
        public void SetNotifyValue(bool enabled, CBCharacteristic characteristic)
        {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
            _nativePeripheral.SetNotifyValue(enabled, characteristic);
        }

        /// <summary>
        /// The connection state of the peripheral.
        /// </summary>
        public CBPeripheralState State
        {
            get
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                return _nativePeripheral.State;
            }
        }

        internal CBCharacteristic FindCharacteristic(string serviceUUID, string characteristicUUID)
        {
            if (string.IsNullOrEmpty(serviceUUID))
            {
                return Services.SelectMany(s => s.Characteristics).FirstOrDefault(c => c.UUID == characteristicUUID);
            }

            var service = Services.FirstOrDefault(s => s.UUID == serviceUUID);
            if (service == null) return null;
            return service.FindCharacteristic(characteristicUUID);
        }

        CBCharacteristic FindOrInitializeCharacteristic(CBService service, string characteristicUUID)
        {
            var characteristic = service.FindCharacteristic(characteristicUUID);
            if (characteristic == null)
            {
                var nativeCharacteristicProxy = new NativeCharacteristicProxy(service.UUID, characteristicUUID, service.Peripheral.Handle);
                characteristic = new CBCharacteristic(characteristicUUID, nativeCharacteristicProxy);
            }
            return characteristic;
        }

        void INativePeripheralDelegate.DidDiscoverServices(string[] serviceUUIDs, CBError error)
        {
            if (_disposed) return;
            var services = serviceUUIDs.Select(uuid =>
            {
                return _services.FirstOrDefault(s => s.UUID == uuid) ?? new CBService(uuid, this);
            }).ToArray();
            _services.Clear();
            _services.AddRange(services);

            Delegate?.DidDiscoverServices(this, error);
        }

        void INativePeripheralDelegate.DidDiscoverCharacteristics(string serviceUUID, string[] characteristicUUIDs, CBError error)
        {
            if (_disposed) return;
            var service = _services.FirstOrDefault(s => s.UUID == serviceUUID);
            if (service == null) return;
            var characteristics = characteristicUUIDs.Select(uuid => FindOrInitializeCharacteristic(service, uuid)).ToArray();
            service.UpdateCharacteristics(characteristics);
            Delegate?.DidDiscoverCharacteristics(this, service, error);
        }

        void INativePeripheralDelegate.DidUpdateValueForCharacteristic(string serviceUUID, string characteristicUUID, byte[] data, CBError error)
        {
            if (_disposed) return;
            var characteristic = FindCharacteristic(serviceUUID, characteristicUUID);
            if (characteristic == null) return;
            characteristic.UpdateValue(data);
            Delegate?.DidUpdateValueForCharacteristic(this, characteristic, error);
        }

        void INativePeripheralDelegate.DidWriteValueForCharacteristic(string serviceUUID, string characteristicUUID, CBError error)
        {
            if (_disposed) return;
            var characteristic = FindCharacteristic(serviceUUID, characteristicUUID);
            if (characteristic == null) return;

            Delegate?.DidWriteValueForCharacteristic(this, characteristic, error);
        }

        void INativePeripheralDelegate.DidUpdateNotificationStateForCharacteristic(string serviceUUID, string characteristicUUID, bool isNotifying, CBError error)
        {
            if (_disposed) return;
            var characteristic = FindCharacteristic(serviceUUID, characteristicUUID);
            if (characteristic == null) return;
            characteristic.UpdateIsNotifying(isNotifying);
            Delegate?.DidUpdateNotificationStateForCharacteristic(this, characteristic, error);
        }

        public override string ToString()
        {
            return $"CBPeripheral: identifier = {Identifier}, name = {Name}, state = {State}";
        }

        public void Dispose()
        {
            if (_disposed) return;

            Handle?.Dispose();

            _disposed = true;
        }
    }
}
