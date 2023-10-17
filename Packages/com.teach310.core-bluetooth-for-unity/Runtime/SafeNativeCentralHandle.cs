using System;
using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    internal class SafeNativeCentralHandle : SafeHandle
    {
        public override bool IsInvalid => handle == IntPtr.Zero;

        internal SafeNativeCentralHandle(IntPtr handle) : base(handle, true) { }

        protected override bool ReleaseHandle()
        {
            NativeMethods.cb4u_central_release(handle);
            return true;
        }
    }
}
