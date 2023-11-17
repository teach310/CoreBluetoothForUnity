using System;
using CoreBluetooth.Foundation;
using Microsoft.Win32.SafeHandles;

namespace CoreBluetooth
{
    internal class SafeNativePeripheralManagerHandle : SafeHandleZeroOrMinusOneIsInvalid
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
            AnyObject.Release(handle);
            return true;
        }
    }
}
