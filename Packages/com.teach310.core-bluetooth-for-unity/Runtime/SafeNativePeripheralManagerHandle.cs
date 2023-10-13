using System;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;

namespace CoreBluetooth
{
    public class SafeNativePeripheralManagerHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        static Dictionary<IntPtr, CBPeripheralManager> s_peripheralManagerMap = new Dictionary<IntPtr, CBPeripheralManager>();

        SafeNativePeripheralManagerHandle() : base(true) { }

        internal static SafeNativePeripheralManagerHandle Create(CBPeripheralManager peripheralManager)
        {
            var instance = NativeMethods.cb4u_peripheral_manager_new();
            RegisterHandlers(instance);
            s_peripheralManagerMap.Add(instance.handle, peripheralManager);
            return instance;
        }

        protected override bool ReleaseHandle()
        {
            s_peripheralManagerMap.Remove(handle);
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

        static CBPeripheralManager GetPeripheralManager(IntPtr peripheralPtr)
        {
            if (!s_peripheralManagerMap.TryGetValue(peripheralPtr, out var peripheralManager))
            {
                UnityEngine.Debug.LogError("CBPeripheralManager instance not found.");
            }
            return peripheralManager;
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidUpdateStateHandler))]
        internal static void DidUpdateState(IntPtr peripheralPtr, CBManagerState state)
        {
            GetPeripheralManager(peripheralPtr)?.DidUpdateState(state);
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidAddServiceHandler))]
        internal static void DidAddService(IntPtr peripheralPtr, string serviceUUID, int errorCode)
        {
            GetPeripheralManager(peripheralPtr)?.DidAddService(serviceUUID, CBError.CreateOrNullFromCode(errorCode));
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidStartAdvertisingHandler))]
        internal static void DidStartAdvertising(IntPtr peripheralPtr, int errorCode)
        {
            GetPeripheralManager(peripheralPtr)?.DidStartAdvertising(CBError.CreateOrNullFromCode(errorCode));
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidReceiveReadRequestHandler))]
        internal static void DidReceiveReadRequest(IntPtr peripheralPtr, IntPtr requestPtr)
        {
            GetPeripheralManager(peripheralPtr)?.DidReceiveReadRequest(
                new SafeNativeATTRequestHandle(requestPtr)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidReceiveWriteRequestsHandler))]
        internal static void DidReceiveWriteRequests(IntPtr peripheralPtr, IntPtr requestsPtr)
        {
            GetPeripheralManager(peripheralPtr)?.DidReceiveWriteRequests(
                new SafeNativeATTRequestsHandle(requestsPtr)
            );
        }
    }
}
