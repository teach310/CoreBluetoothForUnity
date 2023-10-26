using System;

namespace CoreBluetooth.Foundation
{
    internal class SafeNSMutableDictionaryHandle : SafeNSObjectHandle
    {
        public SafeNSMutableDictionaryHandle() : base() { }
        public SafeNSMutableDictionaryHandle(IntPtr handle) : base(handle) { }
    }
}
