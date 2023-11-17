using System;
using System.Runtime.InteropServices;
using CoreBluetooth.Foundation;

namespace CoreBluetooth
{
    internal class SafeNativeCentralHandle : SafeHandle
    {
        public override bool IsInvalid => handle == IntPtr.Zero;

        internal SafeNativeCentralHandle(IntPtr handle) : base(handle, true) { }

        protected override bool ReleaseHandle()
        {
            AnyObject.Release(handle);
            return true;
        }
    }
}
