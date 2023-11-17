using System;

namespace CoreBluetooth.Foundation
{
    internal static class AnyObject
    {
        public static void Release(IntPtr handle)
        {
            if (handle != IntPtr.Zero)
            {
                NativeMethods.any_object_release(handle);
            }
        }
    }
}
