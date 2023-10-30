using System;

namespace CoreBluetooth.Foundation
{
    public class NSNumber : IDisposable
    {
        internal SafeNSNumberHandle Handle { get; private set; }

        internal NSNumber(bool value)
        {
            Handle = NativeMethods.ns_number_new_bool(value);
        }

        internal NSNumber(int value)
        {
            Handle = NativeMethods.ns_number_new_int(value);
        }

        internal NSNumber(SafeNSNumberHandle handle)
        {
            Handle = handle;
        }

        internal NSNumber(IntPtr handle)
        {
            Handle = new SafeNSNumberHandle(handle);
        }

        public bool BoolValue()
        {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(Handle.IsInvalid, this);
            return NativeMethods.ns_number_bool_value(Handle);
        }

        public int IntValue()
        {
            ExceptionUtils.ThrowObjectDisposedExceptionIf(Handle.IsInvalid, this);
            return NativeMethods.ns_number_int_value(Handle);
        }

        public void Dispose()
        {
            if (Handle != null && !Handle.IsInvalid)
                Handle.Dispose();
        }
    }
}
