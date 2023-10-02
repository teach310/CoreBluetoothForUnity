namespace CoreBluetooth
{
    internal class NativeMutableCharacteristicProxy : INativeMutableCharacteristic
    {
        SafeNativeMutableCharacteristicHandle _handle;

        internal NativeMutableCharacteristicProxy(SafeNativeMutableCharacteristicHandle handle, byte[] value = null)
        {
            _handle = handle;
        }

        void INativeMutableCharacteristic.SetValue(byte[] value)
        {
            NativeMethods.cb4u_mutable_characteristic_set_value(_handle, value, value?.Length ?? 0);
        }

        CBCharacteristicProperties INativeCharacteristic.Properties
        {
            get
            {
                int result = NativeMethods.cb4u_mutable_characteristic_properties(_handle);
                return (CBCharacteristicProperties)result;
            }
        }

        CBAttributePermissions INativeMutableCharacteristic.Permissions
        {
            get
            {
                int result = NativeMethods.cb4u_mutable_characteristic_permissions(_handle);
                return (CBAttributePermissions)result;
            }
        }

        void INativeMutableCharacteristic.SetProperties(CBCharacteristicProperties properties)
        {
            NativeMethods.cb4u_mutable_characteristic_set_properties(_handle, (int)properties);
        }

        void INativeMutableCharacteristic.SetPermissions(CBAttributePermissions permissions)
        {
            NativeMethods.cb4u_mutable_characteristic_set_permissions(_handle, (int)permissions);
        }
    }
}
