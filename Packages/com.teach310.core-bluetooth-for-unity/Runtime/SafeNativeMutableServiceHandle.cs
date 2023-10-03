using System;
using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    internal class SafeNativeMutableServiceHandle : SafeHandle
    {
        public override bool IsInvalid => handle == IntPtr.Zero;

        SafeNativeMutableServiceHandle(IntPtr handle) : base(handle, true) { }

        internal static SafeNativeMutableServiceHandle Create(string uuid, bool isPrimary)
        {
            var handle = NativeMethods.cb4u_mutable_service_new(uuid, isPrimary);
            var instance = new SafeNativeMutableServiceHandle(handle);
            return instance;
        }

        protected override bool ReleaseHandle()
        {
            NativeMethods.cb4u_mutable_service_release(handle);
            return true;
        }
    }
}
