using System;
using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    internal class SafeNativeMutableCharacteristicHandle : SafeHandle
    {
        public override bool IsInvalid => handle == IntPtr.Zero;

        SafeNativeMutableCharacteristicHandle(IntPtr handle) : base(handle, true) { }

        internal static SafeNativeMutableCharacteristicHandle Create(
            string uuid,
            CBCharacteristicProperties properties,
            byte[] value,
            CBAttributePermissions permissions
        )
        {
            var handle = NativeMethods.cb4u_mutable_characteristic_new(uuid,
                (int)properties,
                value,
                value?.Length ?? 0,
                (int)permissions);
            var instance = new SafeNativeMutableCharacteristicHandle(handle);
            return instance;
        }

        protected override bool ReleaseHandle()
        {
            NativeMethods.cb4u_mutable_characteristic_release(handle);
            return true;
        }
    }
}
