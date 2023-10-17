namespace CoreBluetooth
{
    internal class NativeMutableCharacteristicProxy
    {
        SafeNativeMutableCharacteristicHandle _handle;

        internal NativeMutableCharacteristicProxy(SafeNativeMutableCharacteristicHandle handle, byte[] value = null)
        {
            _handle = handle;
        }

        internal byte[] Value
        {
            get
            {
                var dataLength = NativeMethods.cb4u_mutable_characteristic_value_length(_handle);
                var data = new byte[dataLength];
                int result = NativeMethods.cb4u_mutable_characteristic_value(_handle, data, dataLength);

                if (result == 0)
                {
                    return null;
                }
                return data;
            }
        }

        internal void SetValue(byte[] value)
        {
            NativeMethods.cb4u_mutable_characteristic_set_value(_handle, value, value?.Length ?? 0);
        }

        internal CBCharacteristicProperties Properties
        {
            get
            {
                int result = NativeMethods.cb4u_mutable_characteristic_properties(_handle);
                return (CBCharacteristicProperties)result;
            }
        }

        internal CBAttributePermissions Permissions
        {
            get
            {
                int result = NativeMethods.cb4u_mutable_characteristic_permissions(_handle);
                return (CBAttributePermissions)result;
            }
        }

        internal void SetProperties(CBCharacteristicProperties properties)
        {
            NativeMethods.cb4u_mutable_characteristic_set_properties(_handle, (int)properties);
        }

        internal void SetPermissions(CBAttributePermissions permissions)
        {
            NativeMethods.cb4u_mutable_characteristic_set_permissions(_handle, (int)permissions);
        }
    }
}
