using System;
using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    public class SafeNativeATTRequestHandle : SafeHandle
    {
        public override bool IsInvalid => handle == IntPtr.Zero;

        internal SafeNativeATTRequestHandle(IntPtr handle) : base(handle, true) { }

        protected override bool ReleaseHandle()
        {
            NativeMethods.cb4u_att_request_release(handle);
            return true;
        }
    }
}