
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    internal class NativePeripheralManagerProxy : IDisposable
    {
        static Dictionary<IntPtr, INativePeripheralManagerDelegate> s_peripheralManagerDelegateMap = new Dictionary<IntPtr, INativePeripheralManagerDelegate>();

        readonly SafeNativePeripheralManagerHandle _handle;

        internal NativePeripheralManagerProxy(SafeNativePeripheralManagerHandle handle, INativePeripheralManagerDelegate peripheralManagerDelegate)
        {
            _handle = handle;
            if (peripheralManagerDelegate != null)
            {
                s_peripheralManagerDelegateMap[handle.DangerousGetHandle()] = peripheralManagerDelegate;
            }

            RegisterHandlers();
        }

        internal void AddService(SafeNativeMutableServiceHandle service)
        {
            NativeMethods.cb4u_peripheral_manager_add_service(_handle, service);
        }

        internal void RemoveService(SafeNativeMutableServiceHandle service)
        {
            NativeMethods.cb4u_peripheral_manager_remove_service(_handle, service);
        }

        internal void RemoveAllServices()
        {
            NativeMethods.cb4u_peripheral_manager_remove_all_services(_handle);
        }

        internal void StartAdvertising(StartAdvertisingOptions options = null)
        {
            if (options != null)
            {
                if (options.ServiceUUIDs != null)
                {
                    foreach (string uuidString in options.ServiceUUIDs)
                    {
                        ExceptionUtils.ThrowArgumentExceptionIfInvalidCBUUID(uuidString, nameof(options.ServiceUUIDs));
                    }
                }
            }
            else
            {
                options = new StartAdvertisingOptions();
            }

            NativeMethods.cb4u_peripheral_manager_start_advertising(
                _handle,
                options.LocalName,
                options.ServiceUUIDs,
                options.ServiceUUIDs?.Length ?? 0
            );
        }

        internal void StopAdvertising()
        {
            NativeMethods.cb4u_peripheral_manager_stop_advertising(_handle);
        }

        internal bool IsAdvertising => NativeMethods.cb4u_peripheral_manager_is_advertising(_handle);

        internal bool UpdateValue(byte[] value, SafeNativeMutableCharacteristicHandle characteristic, SafeNativeCentralHandle[] subscribedCentrals = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            IntPtr[] subscribedCentralsPtr = null;
            if (subscribedCentrals != null)
            {
                subscribedCentralsPtr = new IntPtr[subscribedCentrals.Length];
                for (int i = 0; i < subscribedCentrals.Length; i++)
                {
                    subscribedCentralsPtr[i] = subscribedCentrals[i].DangerousGetHandle();
                }
            }

            return NativeMethods.cb4u_peripheral_manager_update_value(
                _handle,
                value,
                value.Length,
                characteristic,
                subscribedCentralsPtr,
                subscribedCentrals?.Length ?? 0
            );
        }

        internal void RespondToRequest(CBATTRequest request, CBATTError result)
        {
            NativeMethods.cb4u_peripheral_manager_respond_to_request(_handle, request.Handle, (int)result);
        }

        void RegisterHandlers()
        {
            NativeMethods.cb4u_peripheral_manager_register_handlers(
                _handle,
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

        public void Dispose()
        {
            s_peripheralManagerDelegateMap.Remove(_handle.DangerousGetHandle());
        }

        static INativePeripheralManagerDelegate GetDelegate(IntPtr peripheralPtr)
        {
            return s_peripheralManagerDelegateMap.GetValueOrDefault(peripheralPtr);
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidUpdateStateHandler))]
        public static void DidUpdateState(IntPtr peripheralPtr, CBManagerState state)
        {
            GetDelegate(peripheralPtr)?.DidUpdateState(state);
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidAddServiceHandler))]
        public static void DidAddService(IntPtr peripheralPtr, IntPtr serviceUUIDPtr, int errorCode)
        {
            GetDelegate(peripheralPtr)?.DidAddService(
                Marshal.PtrToStringUTF8(serviceUUIDPtr),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidStartAdvertisingHandler))]
        public static void DidStartAdvertising(IntPtr peripheralPtr, int errorCode)
        {
            GetDelegate(peripheralPtr)?.DidStartAdvertising(CBError.CreateOrNullFromCode(errorCode));
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidSubscribeToCharacteristicHandler))]
        public static void DidSubscribeToCharacteristic(IntPtr peripheralPtr, IntPtr centralPtr, IntPtr serviceUUIDPtr, IntPtr characteristicUUIDPtr)
        {
            GetDelegate(peripheralPtr)?.DidSubscribeToCharacteristic(
                new SafeNativeCentralHandle(centralPtr),
                Marshal.PtrToStringUTF8(serviceUUIDPtr),
                Marshal.PtrToStringUTF8(characteristicUUIDPtr)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidUnsubscribeFromCharacteristicHandler))]
        public static void DidUnsubscribeFromCharacteristic(IntPtr peripheralPtr, IntPtr centralPtr, IntPtr serviceUUIDPtr, IntPtr characteristicUUIDPtr)
        {
            GetDelegate(peripheralPtr)?.DidUnsubscribeFromCharacteristic(
                new SafeNativeCentralHandle(centralPtr),
                Marshal.PtrToStringUTF8(serviceUUIDPtr),
                Marshal.PtrToStringUTF8(characteristicUUIDPtr)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerIsReadyToUpdateSubscribersHandler))]
        public static void IsReadyToUpdateSubscribers(IntPtr peripheralPtr)
        {
            GetDelegate(peripheralPtr)?.IsReadyToUpdateSubscribers();
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidReceiveReadRequestHandler))]
        public static void DidReceiveReadRequest(IntPtr peripheralPtr, IntPtr requestPtr)
        {
            GetDelegate(peripheralPtr)?.DidReceiveReadRequest(
                new SafeNativeATTRequestHandle(requestPtr)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidReceiveWriteRequestsHandler))]
        public static void DidReceiveWriteRequests(IntPtr peripheralPtr, IntPtr requestsPtr)
        {
            GetDelegate(peripheralPtr)?.DidReceiveWriteRequests(
                new SafeNativeATTRequestsHandle(requestsPtr)
            );
        }
    }
}
