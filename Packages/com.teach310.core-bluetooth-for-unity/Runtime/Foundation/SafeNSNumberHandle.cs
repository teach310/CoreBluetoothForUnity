using System;

namespace CoreBluetooth.Foundation
{
    internal class SafeNSNumberHandle : SafeNSObjectHandle
    {
        public SafeNSNumberHandle() : base() { }
        public SafeNSNumberHandle(IntPtr handle) : base(handle) { }
    }
}
