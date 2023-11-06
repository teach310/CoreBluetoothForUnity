using System;

namespace CoreBluetooth
{
    /// <summary>
    /// A characteristic of a local peripheral’s service.
    /// https://developer.apple.com/documentation/corebluetooth/cbmutablecharacteristic
    /// </summary>
    public class CBMutableCharacteristic : CBCharacteristic, IDisposable
    {
        bool _disposed = false;
        internal SafeNativeMutableCharacteristicHandle Handle { get; }
        NativeMutableCharacteristicProxy _nativeMutableCharacteristic = null;

        public override byte[] Value
        {
            get
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                return _nativeMutableCharacteristic.Value;
            }
            set
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                _nativeMutableCharacteristic.SetValue(value);
            }
        }

        public override CBCharacteristicProperties Properties
        {
            get
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                return _nativeMutableCharacteristic.Properties;
            }
            set
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                _nativeMutableCharacteristic.SetProperties(value);
            }
        }

        public CBAttributePermissions Permissions
        {
            get
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                return _nativeMutableCharacteristic.Permissions;
            }
            set
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                _nativeMutableCharacteristic.SetPermissions(value);
            }
        }

        public CBMutableCharacteristic(
            string uuid,
            CBCharacteristicProperties properties,
            byte[] value,
            CBAttributePermissions permissions
        ) : base(uuid, null)
        {
            Handle = SafeNativeMutableCharacteristicHandle.Create(uuid, properties, value, permissions);
            _nativeMutableCharacteristic = new NativeMutableCharacteristicProxy(Handle);
        }

        public override string ToString()
        {
            var valueText = Value == null ? "null" : $"{{length = {Value.Length}, bytes = {BitConverter.ToString(Value).Replace("-", "")}}}";
            var notifyingText = IsNotifying ? "YES" : "NO";
            return $"CBMutableCharacteristic: UUID = {UUID}, properties = {Properties}, value = {valueText}, notifying = {notifyingText}, permissions = {Permissions}";
        }

        public void Dispose()
        {
            if (_disposed) return;

            Handle?.Dispose();

            _disposed = true;
        }
    }
}
