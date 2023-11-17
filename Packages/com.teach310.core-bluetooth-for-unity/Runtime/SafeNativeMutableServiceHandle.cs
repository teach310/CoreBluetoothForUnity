using CoreBluetooth.Foundation;
using Microsoft.Win32.SafeHandles;

namespace CoreBluetooth
{
    internal class SafeNativeMutableServiceHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        SafeNativeMutableServiceHandle() : base(true) { }

        internal static SafeNativeMutableServiceHandle Create(string uuid, bool isPrimary)
        {
            return NativeMethods.cb4u_mutable_service_new(uuid, isPrimary);
        }

        protected override bool ReleaseHandle()
        {
            AnyObject.Release(handle);
            return true;
        }
    }
}
