

namespace CoreBluetooth
{
    /// <summary>
    /// A collection of data and associated behaviors that accomplish a function or feature of a device.
    /// https://developer.apple.com/documentation/corebluetooth/cbservice
    /// </summary>
    public class CBService
    {
        public string UUID { get; }

        /// <summary>
        /// The peripheral to which this service belongs.
        /// </summary>
        public CBPeripheral Peripheral { get; }

        public CBService(string uuid, CBPeripheral peripheral)
        {
            this.UUID = uuid;
            this.Peripheral = peripheral;
        }

        public override string ToString() => $"CBService: uuid={UUID}";
    }
}
