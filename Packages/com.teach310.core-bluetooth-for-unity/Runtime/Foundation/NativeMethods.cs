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
        internal static extern void ns_object_release(IntPtr handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern SafeNSNumberHandle ns_number_new_bool([MarshalAs(UnmanagedType.I1)] bool value);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern SafeNSNumberHandle ns_number_new_int(int value);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ns_number_bool_value(SafeNSNumberHandle handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ns_number_int_value(SafeNSNumberHandle handle);
    }
}
