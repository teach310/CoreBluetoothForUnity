

namespace CoreBluetooth
{
    /// <summary>
    /// A collection of data and associated behaviors that accomplish a function or feature of a device.
    /// https://developer.apple.com/documentation/corebluetooth/cbservice
    /// </summary>
    public class CBService
    {
        public string uuid { get; }

        /// <summary>
        /// The peripheral to which this service belongs.
        /// </summary>
        public CBPeripheral peripheral { get; }

        public CBService(string uuid, CBPeripheral peripheral)
        {
            this.uuid = uuid;
            this.peripheral = peripheral;
        }

        public override string ToString() => $"CBService: uuid={uuid}";
    }
}
