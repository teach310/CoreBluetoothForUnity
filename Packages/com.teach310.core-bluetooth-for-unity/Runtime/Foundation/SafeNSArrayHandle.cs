using System;

namespace CoreBluetooth.Foundation
{
    internal class SafeNSArrayHandle : SafeNSObjectHandle
    {
        public SafeNSArrayHandle() : base() { }
        public SafeNSArrayHandle(IntPtr handle) : base(handle) { }
    }
}
