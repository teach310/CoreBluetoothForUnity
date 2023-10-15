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

    public enum CBCharacteristicWriteType
    {
        WithResponse = 0,
        WithoutResponse
    }

    /// <summary>
    /// A characteristic of a remote peripheral’s service.
    /// https://developer.apple.com/documentation/corebluetooth/cbcharacteristic
    /// </summary>
    public class CBCharacteristic
    {
        static readonly string s_notImplementedExceptionMessage = "Not available on 'CBCharacteristic', only available on CBMutableCharacteristic.";

        public string UUID { get; }

        /// <summary>
        /// The service to which this characteristic belongs.
        /// </summary>
        public CBService Service { get; private set; }
        internal void UpdateService(CBService service) => Service = service;

        byte[] _value = null;
        public virtual byte[] Value
        {
            get => _value;
            set => throw new NotImplementedException(s_notImplementedExceptionMessage);
        }
        internal void UpdateValue(byte[] value) => _value = value;

        public virtual CBCharacteristicProperties Properties
        {
            get => _nativeCharacteristic.Properties;
            set => throw new NotImplementedException(s_notImplementedExceptionMessage);
        }

        public bool IsNotifying { get; private set; } = false;
        internal void UpdateIsNotifying(bool isNotifying) => IsNotifying = isNotifying;

        NativeCharacteristicProxy _nativeCharacteristic;

        internal CBCharacteristic(string uuid, NativeCharacteristicProxy nativeCharacteristic)
        {
            this.UUID = uuid;
            _nativeCharacteristic = nativeCharacteristic;
        }

        public override string ToString()
        {
            var valueText = Value == null ? "null" : $"{{length = {Value.Length}, bytes = {BitConverter.ToString(Value).Replace("-", "")}}}";
            var notifyingText = IsNotifying ? "YES" : "NO";
            return $"CBCharacteristic: UUID = {UUID}, properties = {Properties}, value = {valueText}, notifying = {notifyingText}";
        }
    }
}
