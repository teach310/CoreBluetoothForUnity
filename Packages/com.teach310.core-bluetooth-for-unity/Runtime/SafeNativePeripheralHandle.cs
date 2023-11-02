using System;
using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    internal class SafeNativePeripheralHandle : SafeHandle
    {
        public override bool IsInvalid => handle == IntPtr.Zero;

        internal SafeNativePeripheralHandle(IntPtr handle) : base(handle, true)
        {
        }

        protected override bool ReleaseHandle()
        {
            NativeMethods.cb4u_peripheral_release(handle);
            return true;
        }
    }
}
