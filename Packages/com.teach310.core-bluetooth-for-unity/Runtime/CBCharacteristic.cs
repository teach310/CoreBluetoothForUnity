using System;

namespace CoreBluetooth
{
    [Flags]
    public enum CBCharacteristicProperties
    {
        Broadcast = 1,
        Read = 2,
        WriteWithoutResponse = 4,
        Write = 8,
        Notify = 16,
        Indicate = 32,
        AuthenticatedSignedWrites = 64,
        ExtendedProperties = 128,
        NotifyEncryptionRequired = 256,
        IndicateEncryptionRequired = 512
    }

    internal interface INativeCharacteristic
    {
        CBCharacteristicProperties Properties { get; }
    }

    /// <summary>
    /// A characteristic of a remote peripheral’s service.
    /// https://developer.apple.com/documentation/corebluetooth/cbcharacteristic
    /// </summary>
    public class CBCharacteristic
    {
        public string UUID { get; }

        /// <summary>
        /// The service to which this characteristic belongs.
        /// </summary>
        public CBService Service { get; }

        CBCharacteristicProperties Properties => _nativeCharacteristic.Properties;

        INativeCharacteristic _nativeCharacteristic;

        internal CBCharacteristic(string uuid, CBService service, INativeCharacteristic nativeCharacteristic)
        {
            this.UUID = uuid;
            this.Service = service;
            this._nativeCharacteristic = nativeCharacteristic;
        }

        public override string ToString()
        {
            return $"CBCharacteristic: UUID = {UUID}, properties = {Properties}";
        }
    }
}
