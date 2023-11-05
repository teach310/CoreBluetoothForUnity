using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    internal class NativePeripheralManagerCallbacks : INativePeripheralManagerCallbacks
    {
        readonly static Dictionary<IntPtr, INativePeripheralManagerDelegate> s_peripheralManagerDelegateMap = new Dictionary<IntPtr, INativePeripheralManagerDelegate>();

        public void Register(SafeNativePeripheralManagerHandle handle, INativePeripheralManagerDelegate peripheralManagerDelegate)
        {
            s_peripheralManagerDelegateMap[handle.DangerousGetHandle()] = peripheralManagerDelegate;
            NativeMethods.cb4u_peripheral_manager_register_handlers(
                handle,
                DidUpdateState,
                DidAddService,
                DidStartAdvertising,
                DidSubscribeToCharacteristic,
                DidUnsubscribeFromCharacteristic,
                IsReadyToUpdateSubscribers,
                DidReceiveReadRequest,
                DidReceiveWriteRequests
            );
        }

        public void Unregister(SafeNativePeripheralManagerHandle handle)
        {
            s_peripheralManagerDelegateMap.Remove(handle.DangerousGetHandle());
        }

        static INativePeripheralManagerDelegate GetDelegate(IntPtr peripheralPtr)
        {
            return s_peripheralManagerDelegateMap.GetValueOrDefault(peripheralPtr);
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidUpdateStateHandler))]
        static void DidUpdateState(IntPtr peripheralPtr, CBManagerState state)
        {
            GetDelegate(peripheralPtr)?.DidUpdateState(state);
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidAddServiceHandler))]
        static void DidAddService(IntPtr peripheralPtr, IntPtr serviceUUIDPtr, int errorCode)
        {
            GetDelegate(peripheralPtr)?.DidAddService(
                Marshal.PtrToStringUTF8(serviceUUIDPtr),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidStartAdvertisingHandler))]
        static void DidStartAdvertising(IntPtr peripheralPtr, int errorCode)
        {
            GetDelegate(peripheralPtr)?.DidStartAdvertising(CBError.CreateOrNullFromCode(errorCode));
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidSubscribeToCharacteristicHandler))]
        static void DidSubscribeToCharacteristic(IntPtr peripheralPtr, IntPtr centralPtr, IntPtr serviceUUIDPtr, IntPtr characteristicUUIDPtr)
        {
            GetDelegate(peripheralPtr)?.DidSubscribeToCharacteristic(
                new SafeNativeCentralHandle(centralPtr),
                Marshal.PtrToStringUTF8(serviceUUIDPtr),
                Marshal.PtrToStringUTF8(characteristicUUIDPtr)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidUnsubscribeFromCharacteristicHandler))]
        static void DidUnsubscribeFromCharacteristic(IntPtr peripheralPtr, IntPtr centralPtr, IntPtr serviceUUIDPtr, IntPtr characteristicUUIDPtr)
        {
            GetDelegate(peripheralPtr)?.DidUnsubscribeFromCharacteristic(
                new SafeNativeCentralHandle(centralPtr),
                Marshal.PtrToStringUTF8(serviceUUIDPtr),
                Marshal.PtrToStringUTF8(characteristicUUIDPtr)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerIsReadyToUpdateSubscribersHandler))]
        static void IsReadyToUpdateSubscribers(IntPtr peripheralPtr)
        {
            GetDelegate(peripheralPtr)?.IsReadyToUpdateSubscribers();
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidReceiveReadRequestHandler))]
        static void DidReceiveReadRequest(IntPtr peripheralPtr, IntPtr requestPtr)
        {
            GetDelegate(peripheralPtr)?.DidReceiveReadRequest(
                new SafeNativeATTRequestHandle(requestPtr)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidReceiveWriteRequestsHandler))]
        static void DidReceiveWriteRequests(IntPtr peripheralPtr, IntPtr requestsPtr)
        {
            GetDelegate(peripheralPtr)?.DidReceiveWriteRequests(
                new SafeNativeATTRequestsHandle(requestsPtr)
            );
        }
    }
}
