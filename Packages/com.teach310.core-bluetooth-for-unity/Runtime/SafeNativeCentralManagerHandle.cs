using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    public class SafeNativeCentralManagerHandle : SafeHandle
    {
        static Dictionary<IntPtr, CBCentralManager> s_centralManagerMap = new Dictionary<IntPtr, CBCentralManager>();

        public override bool IsInvalid => handle == IntPtr.Zero;

        SafeNativeCentralManagerHandle(IntPtr handle) : base(handle, true) { }

        internal static SafeNativeCentralManagerHandle Create(CBCentralManager centralManager)
        {
            var handle = NativeMethods.cb4u_central_manager_new();
            var instance = new SafeNativeCentralManagerHandle(handle);
            RegisterHandlers(instance);
            s_centralManagerMap.Add(handle, centralManager);
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
                OnDidConnect,
                OnDidDisconnectPeripheral,
                OnDidFailToConnect,
                OnDidDiscoverPeripheral,
                OnDidUpdateState
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
        internal static void OnDidConnect(IntPtr centralPtr, IntPtr peripheralIdPtr)
        {
            GetCentralManager(centralPtr)?.OnDidConnect(Marshal.PtrToStringUTF8(peripheralIdPtr));
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidDisconnectPeripheralHandler))]
        internal static void OnDidDisconnectPeripheral(IntPtr centralPtr, IntPtr peripheralIdPtr, int errorCode)
        {
            GetCentralManager(centralPtr)?.OnDidDisconnectPeripheral(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidFailToConnectHandler))]
        internal static void OnDidFailToConnect(IntPtr centralPtr, IntPtr peripheralIdPtr, int errorCode)
        {
            GetCentralManager(centralPtr)?.OnDidFailToConnect(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidDiscoverPeripheralHandler))]
        internal static void OnDidDiscoverPeripheral(IntPtr centralPtr, IntPtr peripheralIdPtr, IntPtr peripheralNamePtr, int rssi)
        {
            GetCentralManager(centralPtr)?.OnDidDiscoverPeripheral(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                Marshal.PtrToStringUTF8(peripheralNamePtr),
                rssi
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidUpdateStateHandler))]
        internal static void OnDidUpdateState(IntPtr centralPtr, CBManagerState state)
        {
            GetCentralManager(centralPtr)?.OnDidUpdateState(state);
        }
    }
}
