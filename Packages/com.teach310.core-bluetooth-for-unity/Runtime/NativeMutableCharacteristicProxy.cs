namespace CoreBluetooth
{
    internal class NativeMutableCharacteristicProxy : INativeMutableCharacteristic
    {
        SafeNativeMutableCharacteristicHandle _handle;

        internal NativeMutableCharacteristicProxy(SafeNativeMutableCharacteristicHandle handle)
        {
            _handle = handle;
        }

        void INativeMutableCharacteristic.SetValue(byte[] value)
        {
            throw new System.NotImplementedException("TODO");
        }

        CBCharacteristicProperties INativeCharacteristic.Properties
        {
            get
            {
                throw new System.NotImplementedException("TODO");
            }
        }

        void INativeMutableCharacteristic.SetProperties(CBCharacteristicProperties properties)
        {
            throw new System.NotImplementedException("TODO");
        }

        void INativeMutableCharacteristic.SetPermissions(CBAttributePermissions permissions)
        {
            throw new System.NotImplementedException("TODO");
        }
    }
}
