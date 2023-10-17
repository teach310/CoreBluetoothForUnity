using Microsoft.Win32.SafeHandles;

namespace CoreBluetooth
{
    internal class SafeNativeMutableCharacteristicHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        SafeNativeMutableCharacteristicHandle() : base(true) { }

        internal static SafeNativeMutableCharacteristicHandle Create(
            string uuid,
            CBCharacteristicProperties properties,
            byte[] value,
            CBAttributePermissions permissions
        )
        {
            return NativeMethods.cb4u_mutable_characteristic_new(
                uuid,
                (int)properties,
                value,
                value?.Length ?? 0,
                (int)permissions);
        }

        protected override bool ReleaseHandle()
        {
            NativeMethods.cb4u_mutable_characteristic_release(handle);
            return true;
        }
    }
}
