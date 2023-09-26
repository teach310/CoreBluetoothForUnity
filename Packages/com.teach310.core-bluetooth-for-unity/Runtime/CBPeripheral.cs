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

    internal interface INativePeripheral
    {
        void DiscoverServices(string[] serviceUUIDs);
        void DiscoverCharacteristics(string[] characteristicUUIDs, CBService service);
        void ReadValue(CBCharacteristic characteristic);
        void WriteValue(byte[] data, CBCharacteristic characteristic, CBCharacteristicWriteType type);
        void SetNotifyValue(bool enabled, CBCharacteristic characteristic);
        CBPeripheralState State { get; }
    }

    public interface ICBPeripheralDelegate
    {
        void DiscoveredService(CBPeripheral peripheral, CBError error) { }
        void DiscoveredCharacteristic(CBPeripheral peripheral, CBService service, CBError error) { }
        void UpdatedCharacteristicValue(CBPeripheral peripheral, CBCharacteristic characteristic, CBError error) { }
        void WroteCharacteristicValue(CBPeripheral peripheral, CBCharacteristic characteristic, CBError error) { }
        void UpdatedNotificationState(CBPeripheral peripheral, CBCharacteristic characteristic, CBError error) { }
    }

    /// <summary>
    /// A remote peripheral device.
    /// https://developer.apple.com/documentation/corebluetooth/cbperipheral
    /// </summary>
    public class CBPeripheral
    {
        public string Identifier { get; }
        public string Name { get; }
        public ICBPeripheralDelegate Delegate { get; set; }
        List<CBService> _services = new List<CBService>();
        public ReadOnlyCollection<CBService> Services { get; }

        INativePeripheral _nativePeripheral;

        internal CBPeripheral(string id, string name, INativePeripheral nativePeripheral)
        {
            this.Identifier = id;
            this.Name = name;
            this._nativePeripheral = nativePeripheral;
            this.Services = _services.AsReadOnly();
        }

        /// <summary>
        /// Discovers the specified services of the peripheral.
        /// If the servicesUUIDs parameter is nil, this method returns all of the peripheral’s available services. This is much slower than providing an array of service UUIDs to search for.
        /// </summary>
        public void DiscoverServices(string[] serviceUUIDs = null) => _nativePeripheral.DiscoverServices(serviceUUIDs);

        /// <summary>
        /// Discovers the specified characteristics of a service.
        /// </summary>
        public void DiscoverCharacteristics(string[] characteristicUUIDs, CBService service) => _nativePeripheral.DiscoverCharacteristics(characteristicUUIDs, service);

        /// <summary>
        /// Discover all characteristics in a service (slow).
        /// </summary>
        public void DiscoverCharacteristics(CBService service) => DiscoverCharacteristics(null, service);

        /// <summary>
        /// Retrieves the value of a specified characteristic.
        /// When you call this method to read the value of a characteristic, the peripheral calls the peripheral(_:didUpdateValueFor:error:) method of its delegate object.
        /// If the peripheral successfully reads the value of the characteristic, you can access it through the characteristic’s value property.
        /// </summary>
        public void ReadValue(CBCharacteristic characteristic) => _nativePeripheral.ReadValue(characteristic);

        public void WriteValue(byte[] data, CBCharacteristic characteristic, CBCharacteristicWriteType type)
        {
            _nativePeripheral.WriteValue(data, characteristic, type);
        }

        /// <summary>
        /// Sets notifications or indications for the value of a specified characteristic.
        /// </summary>
        public void SetNotifyValue(bool enabled, CBCharacteristic characteristic)
        {
            _nativePeripheral.SetNotifyValue(enabled, characteristic);
        }

        /// <summary>
        /// The connection state of the peripheral.
        /// </summary>
        public CBPeripheralState State => _nativePeripheral.State;

        internal CBCharacteristic FindCharacteristic(string serviceUUID, string characteristicUUID)
        {
            if (string.IsNullOrEmpty(serviceUUID))
            {
                return Services.SelectMany(s => s.Characteristics).FirstOrDefault(c => c.UUID == characteristicUUID);
            }

            var service = Services.FirstOrDefault(s => s.UUID == serviceUUID);
            if (service == null) return null;
            return service.Characteristics.FirstOrDefault(c => c.UUID == characteristicUUID);
        }

        internal void DidDiscoverServices(CBService[] services, CBError error)
        {
            _services.Clear();
            _services.AddRange(services);
            Delegate?.DiscoveredService(this, error);
        }

        internal void DidDiscoverCharacteristics(CBCharacteristic[] characteristics, CBService service, CBError error)
        {
            service.UpdateCharacteristics(characteristics);
            Delegate?.DiscoveredCharacteristic(this, service, error);
        }

        internal void DidUpdateValueForCharacteristic(CBCharacteristic characteristic, byte[] data, CBError error)
        {
            characteristic.UpdateValue(data);
            Delegate?.UpdatedCharacteristicValue(this, characteristic, error);
        }

        internal void DidWriteValueForCharacteristic(CBCharacteristic characteristic, CBError error)
        {
            Delegate?.WroteCharacteristicValue(this, characteristic, error);
        }

        internal void DidUpdateNotificationStateForCharacteristic(CBCharacteristic characteristic, bool isNotifying, CBError error)
        {
            characteristic.UpdateIsNotifying(isNotifying);
            Delegate?.UpdatedNotificationState(this, characteristic, error);
        }

        public override string ToString()
        {
            return $"CBPeripheral: identifier = {Identifier}, name = {Name}, state = {State}";
        }
    }
}
