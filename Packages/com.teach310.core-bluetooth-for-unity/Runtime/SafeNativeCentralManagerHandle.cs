using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace CoreBluetooth
{
    internal interface INativeCentralManagerDelegate
    {
        void DidConnect(string peripheralId) { }
        void DidDisconnectPeripheral(string peripheralId, CBError error) { }
        void DidFailToConnect(string peripheralId, CBError error) { }
        void DidDiscoverPeripheral(SafeNativePeripheralHandle peripheral, int rssi) { }
        void DidUpdateState(CBManagerState state) { }
    }

    internal class SafeNativeCentralManagerHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        static Dictionary<IntPtr, INativeCentralManagerDelegate> s_centralManagerDelegateMap = new Dictionary<IntPtr, INativeCentralManagerDelegate>();

        SafeNativeCentralManagerHandle() : base(true) { }

        static SafeNativeCentralManagerHandle Create(IntPtr options)
        {
            var instance = NativeMethods.cb4u_central_manager_new(options);
            instance.RegisterHandlers();
            return instance;
        }

        internal static SafeNativeCentralManagerHandle Create(Foundation.SafeNSMutableDictionaryHandle options)
        {
            return Create(options.DangerousGetHandle());
        }

        internal static SafeNativeCentralManagerHandle Create()
        {
            return Create(IntPtr.Zero);
        }

        void RegisterHandlers()
        {
            NativeMethods.cb4u_central_manager_register_handlers(
                this,
                DidConnect,
                DidDisconnectPeripheral,
                DidFailToConnect,
                DidDiscoverPeripheral,
                DidUpdateState
            );
        }

        internal void SetDelegate(INativeCentralManagerDelegate centralManagerDelegate)
        {
            s_centralManagerDelegateMap[handle] = centralManagerDelegate;
        }

        protected override bool ReleaseHandle()
        {
            s_centralManagerDelegateMap.Remove(handle);
            NativeMethods.cb4u_central_manager_release(handle);
            return true;
        }

        static INativeCentralManagerDelegate GetDelegate(IntPtr centralPtr)
        {
            if (!s_centralManagerDelegateMap.TryGetValue(centralPtr, out var centralManagerDelegate))
            {
                return null;
            }
            return centralManagerDelegate;
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidConnectHandler))]
        internal static void DidConnect(IntPtr centralPtr, IntPtr peripheralIdPtr)
        {
            GetDelegate(centralPtr)?.DidConnect(Marshal.PtrToStringUTF8(peripheralIdPtr));
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidDisconnectPeripheralHandler))]
        internal static void DidDisconnectPeripheral(IntPtr centralPtr, IntPtr peripheralIdPtr, int errorCode)
        {
            GetDelegate(centralPtr)?.DidDisconnectPeripheral(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidFailToConnectHandler))]
        internal static void DidFailToConnect(IntPtr centralPtr, IntPtr peripheralIdPtr, int errorCode)
        {
            GetDelegate(centralPtr)?.DidFailToConnect(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidDiscoverPeripheralHandler))]
        internal static void DidDiscoverPeripheral(IntPtr centralPtr, IntPtr peripheralPtr, int rssi)
        {
            GetDelegate(centralPtr)?.DidDiscoverPeripheral(
                new SafeNativePeripheralHandle(peripheralPtr),
                rssi
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidUpdateStateHandler))]
        internal static void DidUpdateState(IntPtr centralPtr, CBManagerState state)
        {
            GetDelegate(centralPtr)?.DidUpdateState(state);
        }
    }
}
