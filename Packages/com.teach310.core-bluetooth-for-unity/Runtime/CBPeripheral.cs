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
        CBPeripheralState State { get; }
    }

    public interface CBPeripheralDelegate
    {
        void DiscoveredService(CBPeripheral peripheral, CBError error);
    }

    /// <summary>
    /// A remote peripheral device.
    /// https://developer.apple.com/documentation/corebluetooth/cbperipheral
    /// </summary>
    public class CBPeripheral
    {
        public string Identifier { get; }
        public string Name { get; }
        public CBPeripheralDelegate Delegate { get; set; }
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
        /// The connection state of the peripheral.
        /// </summary>
        public CBPeripheralState state => _nativePeripheral.State;

        internal void DidDiscoverServices(CBService[] services, CBError error)
        {
            _services.Clear();
            _services.AddRange(services);
            Delegate?.DiscoveredService(this, error);
        }

        public override string ToString()
        {
            return $"CBPeripheral: identifier = {Identifier}, name = {Name}, state = {state}";
        }
    }
}
