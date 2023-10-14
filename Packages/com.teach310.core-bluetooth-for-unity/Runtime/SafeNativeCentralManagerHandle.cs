using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace CoreBluetooth
{
    internal class SafeNativeCentralManagerHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        static Dictionary<IntPtr, CBCentralManager> s_centralManagerMap = new Dictionary<IntPtr, CBCentralManager>();

        SafeNativeCentralManagerHandle() : base(true) { }

        internal static SafeNativeCentralManagerHandle Create(CBCentralManager centralManager)
        {
            var instance = NativeMethods.cb4u_central_manager_new();
            RegisterHandlers(instance);
            s_centralManagerMap.Add(instance.handle, centralManager);
            return instance;
        }

        protected override bool ReleaseHandle()
        {
            s_centralManagerMap.Remove(handle);
            NativeMethods.cb4u_central_manager_release(handle);
            return true;
        }

        static void RegisterHandlers(SafeNativeCentralManagerHandle handle)
        {
            NativeMethods.cb4u_central_manager_register_handlers(
                handle,
                DidConnect,
                DidDisconnectPeripheral,
                DidFailToConnect,
                DidDiscoverPeripheral,
                DidUpdateState
            );
        }

        static CBCentralManager GetCentralManager(IntPtr centralPtr)
        {
            if (!s_centralManagerMap.TryGetValue(centralPtr, out var centralManager))
            {
                UnityEngine.Debug.LogError("CBCentralManager instance not found.");
            }
            return centralManager;
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidConnectHandler))]
        internal static void DidConnect(IntPtr centralPtr, IntPtr peripheralIdPtr)
        {
            GetCentralManager(centralPtr)?.DidConnect(Marshal.PtrToStringUTF8(peripheralIdPtr));
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidDisconnectPeripheralHandler))]
        internal static void DidDisconnectPeripheral(IntPtr centralPtr, IntPtr peripheralIdPtr, int errorCode)
        {
            GetCentralManager(centralPtr)?.DidDisconnectPeripheral(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidFailToConnectHandler))]
        internal static void DidFailToConnect(IntPtr centralPtr, IntPtr peripheralIdPtr, int errorCode)
        {
            GetCentralManager(centralPtr)?.DidFailToConnect(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidDiscoverPeripheralHandler))]
        internal static void DidDiscoverPeripheral(IntPtr centralPtr, IntPtr peripheralPtr, int rssi)
        {
            GetCentralManager(centralPtr)?.DidDiscoverPeripheral(
                new SafeNativePeripheralHandle(peripheralPtr),
                rssi
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidUpdateStateHandler))]
        internal static void DidUpdateState(IntPtr centralPtr, CBManagerState state)
        {
            GetCentralManager(centralPtr)?.DidUpdateState(state);
        }
    }
}
