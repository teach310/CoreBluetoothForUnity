using System;

namespace CoreBluetooth.Foundation
{
    internal class SafeNSStringHandle : SafeNSObjectHandle
    {
        public SafeNSStringHandle() : base() { }
        public SafeNSStringHandle(IntPtr handle) : base(handle) { }
    }
}
