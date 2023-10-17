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
            NativeMethods.cb4u_mutable_service_release(handle);
            return true;
        }
    }
}
