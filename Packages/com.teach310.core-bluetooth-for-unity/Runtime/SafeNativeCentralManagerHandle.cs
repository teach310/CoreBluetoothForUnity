using System;
using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    public class SafeNativeCentralManagerHandle : SafeHandle
    {
        SafeNativeCentralManagerHandle(IntPtr handle) : base(handle, true) { }

        public override bool IsInvalid => handle == IntPtr.Zero;

        internal static SafeNativeCentralManagerHandle Create(CBCentralManager centralManager)
        {
            var handle = NativeMethods.cb4u_central_manager_new();
            var instance = new SafeNativeCentralManagerHandle(handle);
            return instance;
        }

        protected override bool ReleaseHandle()
        {
            NativeMethods.cb4u_central_manager_release(handle);
            return true;
        }
    }
}
