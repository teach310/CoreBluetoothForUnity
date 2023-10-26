using System;
using System.Linq;

namespace CoreBluetooth.Foundation
{
    internal class NSArray : IDisposable
    {
        public SafeNSArrayHandle Handle { get; private set; }

        public NSArray(SafeNSArrayHandle handle)
        {
            Handle = handle;
        }

        public static NSArray FromStrings(params string[] strings)
        {
            if (strings is null)
                throw new ArgumentNullException(nameof(strings));

            var nsstrings = new NSString[strings.Length];
            for (var i = 0; i < strings.Length; i++)
            {
                if (strings[i] == null)
                    throw new ArgumentNullException(nameof(strings));
                nsstrings[i] = new NSString(strings[i]);
            }

            var nsarray = FromNSObjects(nsstrings.Select(s => s.Handle).ToArray());
            foreach (var nsstring in nsstrings)
            {
                nsstring.Dispose();
            }
            return nsarray;
        }

        public static NSArray FromNSObjects(params SafeNSObjectHandle[] objects)
        {
            if (objects is null)
                throw new ArgumentNullException(nameof(objects));

            var values = objects.Select(o => o.DangerousGetHandle()).ToArray();
            var handle = NativeMethods.ns_array_new(values, values.Length);
            return new NSArray(handle);
        }

        public static IntPtr[] ArrayFromHandle<T>(SafeNSArrayHandle handle)
        {
            if (handle.IsInvalid)
                return null;

            var count = GetCount(handle);
            var array = new IntPtr[count];
            for (var i = 0; i < count; i++)
            {
                var ptr = GetAtIndex(handle, i);
                array[i] = ptr;
            }
            return array;
        }

        public static string[] StringsFromHandle(SafeNSArrayHandle handle)
        {
            var ptrs = ArrayFromHandle<string>(handle);
            var array = new string[ptrs.Length];
            for (var i = 0; i < ptrs.Length; i++)
            {
                using var nsstring = new NSString(ptrs[i]);
                array[i] = nsstring.ToString();
            }
            return array;
        }

        internal static int GetCount(SafeNSArrayHandle handle)
        {
            return NativeMethods.ns_array_count(handle);
        }

        internal static IntPtr GetAtIndex(SafeNSArrayHandle handle, int index)
        {
            return NativeMethods.ns_array_get_at_index(handle, index);
        }

        public void Dispose()
        {
            if (Handle != null && !Handle.IsInvalid)
                Handle.Dispose();
        }
    }
}
