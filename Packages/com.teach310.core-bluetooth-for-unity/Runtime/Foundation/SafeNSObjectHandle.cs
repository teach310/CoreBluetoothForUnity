using System;
using Microsoft.Win32.SafeHandles;

namespace CoreBluetooth.Foundation
{
    public abstract class SafeNSObjectHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        protected SafeNSObjectHandle() : base(true) { }
        protected SafeNSObjectHandle(IntPtr handle) : base(true)
        {
            SetHandle(handle);
        }

        protected override bool ReleaseHandle()
        {
            AnyObject.Release(handle);
            return true;
        }
    }
}
