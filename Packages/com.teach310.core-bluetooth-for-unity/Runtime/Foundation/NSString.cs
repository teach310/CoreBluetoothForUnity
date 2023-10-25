using System;
using System.Runtime.InteropServices;

namespace CoreBluetooth.Foundation
{
    public class NSString : IDisposable
    {
        internal SafeNSStringHandle Handle { get; private set; }

        public NSString(string str)
        {
            if (str is null)
                throw new ArgumentNullException(nameof(str));

            Handle = NativeMethods.ns_string_new(str);
        }

        internal NSString(SafeNSStringHandle handle)
        {
            Handle = handle;
        }

        internal NSString(IntPtr handle)
        {
            Handle = new SafeNSStringHandle(handle);
        }

        public int LengthOfBytesUtf8()
        {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(Handle.IsInvalid, this);
            return NativeMethods.ns_string_length_of_bytes_utf8(Handle);
        }

        public override string ToString()
        {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(Handle.IsInvalid, this);
            return HandleToString(Handle);
        }

        public void Dispose()
        {
            if (Handle != null && !Handle.IsInvalid)
                Handle.Dispose();
        }

        internal static string HandleToString(SafeNSStringHandle handle)
        {
            if (handle.IsInvalid)
                return null;

            NativeMethods.ns_string_get_cstring_and_length(handle, out IntPtr ptr, out int length);
            if (ptr == IntPtr.Zero)
                return null;

            if (length == 0)
                return string.Empty;

            return Marshal.PtrToStringUTF8(ptr, length);
        }
    }
}
