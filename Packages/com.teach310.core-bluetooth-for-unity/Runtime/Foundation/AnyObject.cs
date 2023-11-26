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

        public static string ToString(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return string.Empty;
            }

            using var description = GetDescription(handle);
            return description.ToString();
        }

        internal static NSString GetDescription(IntPtr handle)
        {
            return new NSString(NativeMethods.any_object_to_string(handle));
        }
    }
}
