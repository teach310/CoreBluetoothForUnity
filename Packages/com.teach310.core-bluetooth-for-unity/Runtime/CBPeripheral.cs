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

    /// <summary>
    /// A remote peripheral device.
    /// https://developer.apple.com/documentation/corebluetooth/cbperipheral
    /// </summary>
    public class CBPeripheral
    {
        public string identifier { get; }
        public string name { get; }

        public CBPeripheralState state { get; private set; } = CBPeripheralState.disconnected;
        INativePeripheral _nativePeripheral;

        internal CBPeripheral(string id, string name, INativePeripheral nativePeripheral)
        {
            this.identifier = id;
            this.name = name;
            this._nativePeripheral = nativePeripheral;
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
    }
}
