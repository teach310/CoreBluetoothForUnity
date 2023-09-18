using System;
using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    internal static class NativeMethods
    {
#if UNITY_IOS && !UNITY_EDITOR
        const string DLL_NAME = "__Internal";
#else
        const string DLL_NAME = "CB4UBundle";
#endif
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr cb4u_central_manager_new();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_central_manager_release(IntPtr handle);

        internal delegate void CB4UCentralManagerDidConnectHandler(IntPtr centralPtr, IntPtr peripheralIdPtr);
        internal delegate void CB4UCentralManagerDidDisconnectPeripheralHandler(IntPtr centralPtr, IntPtr peripheralIdPtr, int errorCode);
        internal delegate void CB4UCentralManagerDidFailToConnectHandler(IntPtr centralPtr, IntPtr peripheralIdPtr, int errorCode);
        internal delegate void CB4UCentralManagerDidDiscoverPeripheralHandler(IntPtr centralPtr, IntPtr peripheralIdPtr, IntPtr peripheralNamePtr, int rssi);
        internal delegate void CB4UCentralManagerDidUpdateStateHandler(IntPtr centralPtr, CBManagerState state);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_central_manager_register_handlers(
            SafeNativeCentralManagerHandle handle,
            CB4UCentralManagerDidConnectHandler didConnectHandler,
            CB4UCentralManagerDidDisconnectPeripheralHandler didDisconnectPeripheralHandler,
            CB4UCentralManagerDidFailToConnectHandler didFailToConnectHandler,
            CB4UCentralManagerDidDiscoverPeripheralHandler didDiscoverPeripheralHandler,
            CB4UCentralManagerDidUpdateStateHandler didUpdateStateHandler
        );

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_central_manager_connect(SafeNativeCentralManagerHandle handle, [MarshalAs(UnmanagedType.LPStr), In] string peripheralId);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_central_manager_cancel_peripheral_connection(SafeNativeCentralManagerHandle handle, [MarshalAs(UnmanagedType.LPStr), In] string peripheralId);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_central_manager_scan_for_peripherals(
            SafeNativeCentralManagerHandle handle,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 2)] string[] serviceUUIDs,
            int serviceUUIDsCount
        );

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_central_manager_stop_scan(SafeNativeCentralManagerHandle handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool cb4u_central_manager_is_scanning(SafeNativeCentralManagerHandle handle);
    }
}
