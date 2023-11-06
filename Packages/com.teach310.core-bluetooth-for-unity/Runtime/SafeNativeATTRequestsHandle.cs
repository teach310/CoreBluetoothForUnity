using System;
using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    internal class SafeNativeATTRequestsHandle : SafeHandle
    {
        public override bool IsInvalid => handle == IntPtr.Zero;

        internal SafeNativeATTRequestsHandle(IntPtr handle) : base(handle, true) { }

        protected override bool ReleaseHandle()
        {
            NativeMethods.cb4u_att_requests_release(handle);
            return true;
        }
    }
}
