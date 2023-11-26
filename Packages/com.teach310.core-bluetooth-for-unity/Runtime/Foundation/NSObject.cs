using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace CoreBluetooth.Foundation
{
    public class NSObject
    {
        public static string ToString<T>(T obj, SafeHandle handle)
        {
            return ToString(obj, handle.DangerousGetHandle());
        }

        public static string ToString<T>(T obj, IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return typeof(T).Name;
            }

            using var description = AnyObject.GetDescription(handle);
            string pattern = @"^<.*?:\s+0x[0-9a-f]+|>$";
            var content = Regex.Replace(description.ToString(), pattern, "");
            if (content == string.Empty)
            {
                return typeof(T).Name;
            }

            return $"{obj.GetType().Name}:{content}";
        }
    }
}
