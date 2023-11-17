using System;
using System.Runtime.InteropServices;
using CoreBluetooth.Foundation;

namespace CoreBluetooth
{
    internal class SafeNativeATTRequestHandle : SafeHandle
    {
        public override bool IsInvalid => handle == IntPtr.Zero;

        internal SafeNativeATTRequestHandle(IntPtr handle) : base(handle, true) { }

        protected override bool ReleaseHandle()
        {
            AnyObject.Release(handle);
            return true;
        }
    }
}
