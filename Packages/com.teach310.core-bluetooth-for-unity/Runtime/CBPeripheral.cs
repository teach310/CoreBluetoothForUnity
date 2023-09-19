using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoreBluetooth
{
    public enum CBPeripheralState
    {
        disconnected = 0,
        connecting,
        connected,
        disconnecting
    }

    internal interface INativePeripheral
    {
        void DiscoverServices(string[] serviceUUIDs);
    }

    public interface CBPeripheralDelegate
    {
        void DidDiscoverServices(CBPeripheral peripheral, CBError error);
    }

    /// <summary>
    /// A remote peripheral device.
    /// https://developer.apple.com/documentation/corebluetooth/cbperipheral
    /// </summary>
    public class CBPeripheral
    {
        public string identifier { get; }
        public string name { get; }
        public CBPeripheralDelegate peripheralDelegate { get; set; }
        List<CBService> _services = new List<CBService>();
        public ReadOnlyCollection<CBService> services { get; }

        public CBPeripheralState state { get; private set; } = CBPeripheralState.disconnected;
        INativePeripheral _nativePeripheral;

        internal CBPeripheral(string id, string name, INativePeripheral nativePeripheral)
        {
            this.identifier = id;
            this.name = name;
            this._nativePeripheral = nativePeripheral;
            this.services = _services.AsReadOnly();
        }

        public override string ToString()
        {
            return $"CBPeripheral: identifier = {identifier}, name = {name}, state = {state}";
        }

        /// <summary>
        /// Discovers the specified services of the peripheral.
        /// If the servicesUUIDs parameter is nil, this method returns all of the peripheral’s available services. This is much slower than providing an array of service UUIDs to search for.
        /// </summary>
        public void DiscoverServices(string[] serviceUUIDs = null) => _nativePeripheral.DiscoverServices(serviceUUIDs);

        internal void OnDidDiscoverServices(CBService[] services, CBError error)
        {
            _services.Clear();
            _services.AddRange(services);
            peripheralDelegate?.DidDiscoverServices(this, error);
        }
    }
}
