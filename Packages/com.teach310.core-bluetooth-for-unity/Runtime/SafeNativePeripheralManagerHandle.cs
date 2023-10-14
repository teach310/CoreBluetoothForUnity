using System;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;

namespace CoreBluetooth
{
    internal interface INativePeripheralManagerDelegate
    {
        void DidUpdateState(CBManagerState state) { }
        void DidAddService(string serviceUUID, CBError error) { }
        void DidStartAdvertising(CBError error) { }
        void DidReceiveReadRequest(SafeNativeATTRequestHandle request) { }
        void DidReceiveWriteRequests(SafeNativeATTRequestsHandle requests) { }
    }

    public class SafeNativePeripheralManagerHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        static Dictionary<IntPtr, INativePeripheralManagerDelegate> s_peripheralManagerDelegateMap = new Dictionary<IntPtr, INativePeripheralManagerDelegate>();

        SafeNativePeripheralManagerHandle() : base(true) { }

        internal static SafeNativePeripheralManagerHandle Create()
        {
            var instance = NativeMethods.cb4u_peripheral_manager_new();
            instance.RegisterHandlers();
            return instance;
        }

        void RegisterHandlers()
        {
            NativeMethods.cb4u_peripheral_manager_register_handlers(
                this,
                DidUpdateState,
                DidAddService,
                DidStartAdvertising,
                DidReceiveReadRequest,
                DidReceiveWriteRequests
            );
        }

        internal void SetDelegate(INativePeripheralManagerDelegate peripheralManagerDelegate)
        {
            s_peripheralManagerDelegateMap[handle] = peripheralManagerDelegate;
        }

        protected override bool ReleaseHandle()
        {
            s_peripheralManagerDelegateMap.Remove(handle);
            NativeMethods.cb4u_peripheral_manager_release(handle);
            return true;
        }

        static void RegisterHandlers(SafeNativePeripheralManagerHandle handle)
        {
            NativeMethods.cb4u_peripheral_manager_register_handlers(
                handle,
                DidUpdateState,
                DidAddService,
                DidStartAdvertising,
                DidReceiveReadRequest,
                DidReceiveWriteRequests
            );
        }

        static INativePeripheralManagerDelegate GetDelegate(IntPtr peripheralPtr)
        {
            if (!s_peripheralManagerDelegateMap.TryGetValue(peripheralPtr, out var peripheralManagerDelegate))
            {
                return null;
            }
            return peripheralManagerDelegate;
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidUpdateStateHandler))]
        internal static void DidUpdateState(IntPtr peripheralPtr, CBManagerState state)
        {
            GetDelegate(peripheralPtr)?.DidUpdateState(state);
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidAddServiceHandler))]
        internal static void DidAddService(IntPtr peripheralPtr, string serviceUUID, int errorCode)
        {
            GetDelegate(peripheralPtr)?.DidAddService(serviceUUID, CBError.CreateOrNullFromCode(errorCode));
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidStartAdvertisingHandler))]
        internal static void DidStartAdvertising(IntPtr peripheralPtr, int errorCode)
        {
            GetDelegate(peripheralPtr)?.DidStartAdvertising(CBError.CreateOrNullFromCode(errorCode));
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidReceiveReadRequestHandler))]
        internal static void DidReceiveReadRequest(IntPtr peripheralPtr, IntPtr requestPtr)
        {
            GetDelegate(peripheralPtr)?.DidReceiveReadRequest(
                new SafeNativeATTRequestHandle(requestPtr)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidReceiveWriteRequestsHandler))]
        internal static void DidReceiveWriteRequests(IntPtr peripheralPtr, IntPtr requestsPtr)
        {
            GetDelegate(peripheralPtr)?.DidReceiveWriteRequests(
                new SafeNativeATTRequestsHandle(requestsPtr)
            );
        }
    }
}
