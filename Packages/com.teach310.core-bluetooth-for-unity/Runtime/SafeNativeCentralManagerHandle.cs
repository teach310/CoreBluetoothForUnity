using System;
using CoreBluetooth.Foundation;
using Microsoft.Win32.SafeHandles;

namespace CoreBluetooth
{
    internal class SafeNativeCentralManagerHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        SafeNativeCentralManagerHandle() : base(true) { }

        static SafeNativeCentralManagerHandle Create(IntPtr options)
        {
            return NativeMethods.cb4u_central_manager_new(options);
        }

        public static SafeNativeCentralManagerHandle Create(Foundation.SafeNSMutableDictionaryHandle options)
        {
            return Create(options.DangerousGetHandle());
        }

        public static SafeNativeCentralManagerHandle Create()
        {
            return Create(IntPtr.Zero);
        }

        protected override bool ReleaseHandle()
        {
            AnyObject.Release(handle);
            return true;
        }
    }
}
