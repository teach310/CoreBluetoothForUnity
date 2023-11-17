using System;
using System.Runtime.InteropServices;
using CoreBluetooth.Foundation;

namespace CoreBluetooth
{
    internal class SafeNativeATTRequestsHandle : SafeHandle
    {
        public override bool IsInvalid => handle == IntPtr.Zero;

        internal SafeNativeATTRequestsHandle(IntPtr handle) : base(handle, true) { }

        protected override bool ReleaseHandle()
        {
            AnyObject.Release(handle);
            return true;
        }
    }
}
