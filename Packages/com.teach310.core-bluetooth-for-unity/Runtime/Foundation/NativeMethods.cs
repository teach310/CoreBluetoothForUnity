using System;
using System.Runtime.InteropServices;

namespace CoreBluetooth.Foundation
{
    internal static class NativeMethods
    {
#if UNITY_IOS && !UNITY_EDITOR
        const string DLL_NAME = "__Internal";
#else
        const string DLL_NAME = "libCoreBluetoothForUnity";
#endif

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void any_object_release(IntPtr handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern SafeNSNumberHandle ns_number_new_bool([MarshalAs(UnmanagedType.I1)] bool value);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern SafeNSNumberHandle ns_number_new_int(int value);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ns_number_bool_value(SafeNSNumberHandle handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ns_number_int_value(SafeNSNumberHandle handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern SafeNSStringHandle ns_string_new([MarshalAs(UnmanagedType.LPStr)] string str);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ns_string_length_of_bytes_utf8(SafeNSStringHandle handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ns_string_get_cstring_and_length(SafeNSStringHandle handle, out IntPtr ptr, out int length);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern SafeNSArrayHandle ns_array_new(IntPtr[] values, int count);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ns_array_count(SafeNSArrayHandle handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr ns_array_get_at_index(SafeNSArrayHandle handle, int index);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern SafeNSMutableDictionaryHandle ns_mutable_dictionary_new();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr ns_mutable_dictionary_get_value(SafeNSMutableDictionaryHandle handle, SafeNSObjectHandle key);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ns_mutable_dictionary_set_value(SafeNSMutableDictionaryHandle handle, SafeNSObjectHandle key, SafeNSObjectHandle value);
    }
}
