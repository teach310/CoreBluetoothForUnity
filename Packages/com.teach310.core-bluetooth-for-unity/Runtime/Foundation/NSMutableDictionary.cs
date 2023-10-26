using System;

namespace CoreBluetooth.Foundation
{
    internal class NSMutableDictionary : IDisposable
    {
        public SafeNSMutableDictionaryHandle Handle { get; private set; }

        public NSMutableDictionary()
        {
            Handle = NativeMethods.ns_mutable_dictionary_new();
        }

        public IntPtr GetValue(SafeNSObjectHandle key)
        {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(Handle.IsInvalid, this);
            if (key.IsInvalid)
                throw new ArgumentNullException(nameof(key));

            return NativeMethods.ns_mutable_dictionary_get_value(Handle, key);
        }

        public bool TryGetValue(SafeNSObjectHandle key, out IntPtr value)
        {
            value = GetValue(key);
            return value != IntPtr.Zero;
        }

        public void SetValue(SafeNSObjectHandle key, SafeNSObjectHandle value)
        {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(Handle.IsInvalid, this);
            if (key.IsInvalid)
                throw new ArgumentNullException(nameof(key));

            NativeMethods.ns_mutable_dictionary_set_value(Handle, key, value);
        }

        public void SetValue(string key, SafeNSObjectHandle value)
        {
            using var nsString = new NSString(key);
            SetValue(nsString.Handle, value);
        }

        public void Dispose()
        {
            if (Handle != null && !Handle.IsInvalid)
                Handle.Dispose();
        }
    }
}
