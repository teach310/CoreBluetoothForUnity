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
                DidConnect,
                DidDisconnectPeripheral,
                DidFailToConnect,
                DidDiscoverPeripheral,
                DidUpdateState,
                PeripheralDidDiscoverServices
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
        internal static void DidDiscoverPeripheral(IntPtr centralPtr, IntPtr peripheralIdPtr, IntPtr peripheralNamePtr, int rssi)
        {
            GetCentralManager(centralPtr)?.DidDiscoverPeripheral(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                Marshal.PtrToStringUTF8(peripheralNamePtr),
                rssi
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidUpdateStateHandler))]
        internal static void DidUpdateState(IntPtr centralPtr, CBManagerState state)
        {
            GetCentralManager(centralPtr)?.DidUpdateState(state);
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralDidDiscoverServicesHandler))]
        internal static void PeripheralDidDiscoverServices(IntPtr centralPtr, IntPtr peripheralIdPtr, IntPtr commaSeparatedServiceUUIDsPtr, int errorCode)
        {
            string commaSeparatedServiceUUIDs = Marshal.PtrToStringUTF8(commaSeparatedServiceUUIDsPtr);
            if (string.IsNullOrEmpty(commaSeparatedServiceUUIDs))
            {
                throw new ArgumentException("commaSeparatedServiceUUIDs is null or empty.");
            }

            GetCentralManager(centralPtr)?.PeripheralDidDiscoverServices(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                commaSeparatedServiceUUIDs.Split(','),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }
    }
}
