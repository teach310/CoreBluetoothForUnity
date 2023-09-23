using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        CBPeripheralState State { get; }
    }

    public interface ICBPeripheralDelegate
    {
        void DiscoveredService(CBPeripheral peripheral, CBError error) { }
        void DiscoveredCharacteristic(CBPeripheral peripheral, CBService service, CBError error) { }
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
        /// The connection state of the peripheral.
        /// </summary>
        public CBPeripheralState State => _nativePeripheral.State;

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

        public override string ToString()
        {
            return $"CBPeripheral: identifier = {Identifier}, name = {Name}, state = {State}";
        }
    }
}
