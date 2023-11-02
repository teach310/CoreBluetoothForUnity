using System;
using Microsoft.Win32.SafeHandles;

namespace CoreBluetooth
{
    public class SafeNativePeripheralManagerHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        SafeNativePeripheralManagerHandle() : base(true) { }

        internal static SafeNativePeripheralManagerHandle Create(IntPtr options)
        {
            return NativeMethods.cb4u_peripheral_manager_new(options);
        }

        internal static SafeNativePeripheralManagerHandle Create(Foundation.SafeNSMutableDictionaryHandle options)
        {
            return Create(options.DangerousGetHandle());
        }

        internal static SafeNativePeripheralManagerHandle Create()
        {
            return Create(IntPtr.Zero);
        }

        protected override bool ReleaseHandle()
        {
            NativeMethods.cb4u_peripheral_manager_release(handle);
            return true;
        }
    }
}
