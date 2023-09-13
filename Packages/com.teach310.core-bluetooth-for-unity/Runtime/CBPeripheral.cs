namespace CoreBluetooth
{
    public enum CBPeripheralState
    {
        disconnected = 0,
        connecting,
        connected,
        disconnecting
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

        public CBPeripheral(string id, string name)
        {
            this.identifier = id;
            this.name = name;
        }

        public override string ToString()
        {
            return $"CBPeripheral: identifier = {identifier}, name = {name}, state = {state}";
        }
    }
}
