using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    internal class NativeCentralManagerCallbacks : INativeCentralManagerCallbacks
    {
        readonly static Dictionary<IntPtr, INativeCentralManagerDelegate> s_centralManagerDelegateMap = new Dictionary<IntPtr, INativeCentralManagerDelegate>();

        public void Register(SafeNativeCentralManagerHandle handle, INativeCentralManagerDelegate centralManagerDelegate)
        {
            s_centralManagerDelegateMap[handle.DangerousGetHandle()] = centralManagerDelegate;
            NativeMethods.cb4u_central_manager_register_handlers(
                handle,
                DidConnect,
                DidDisconnectPeripheral,
                DidFailToConnect,
                DidDiscoverPeripheral,
                DidUpdateState
            );
        }

        public void Unregister(SafeNativeCentralManagerHandle handle)
        {
            s_centralManagerDelegateMap.Remove(handle.DangerousGetHandle());
        }

        static INativeCentralManagerDelegate GetDelegate(IntPtr centralPtr)
        {
            return s_centralManagerDelegateMap.GetValueOrDefault(centralPtr);
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidConnectHandler))]
        static void DidConnect(IntPtr centralPtr, IntPtr peripheralIdPtr)
        {
            GetDelegate(centralPtr)?.DidConnect(Marshal.PtrToStringUTF8(peripheralIdPtr));
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidDisconnectPeripheralHandler))]
        static void DidDisconnectPeripheral(IntPtr centralPtr, IntPtr peripheralIdPtr, int errorCode)
        {
            GetDelegate(centralPtr)?.DidDisconnectPeripheral(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidFailToConnectHandler))]
        static void DidFailToConnect(IntPtr centralPtr, IntPtr peripheralIdPtr, int errorCode)
        {
            GetDelegate(centralPtr)?.DidFailToConnect(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidDiscoverPeripheralHandler))]
        static void DidDiscoverPeripheral(IntPtr centralPtr, IntPtr peripheralPtr, int rssi)
        {
            GetDelegate(centralPtr)?.DidDiscoverPeripheral(
                new SafeNativePeripheralHandle(peripheralPtr),
                rssi
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidUpdateStateHandler))]
        static void DidUpdateState(IntPtr centralPtr, CBManagerState state)
        {
            GetDelegate(centralPtr)?.DidUpdateState(state);
        }
    }
}
