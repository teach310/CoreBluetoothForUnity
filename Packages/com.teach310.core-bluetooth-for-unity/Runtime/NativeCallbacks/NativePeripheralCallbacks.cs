using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    internal class NativePeripheralCallbacks : INativePeripheralCallbacks
    {
        readonly static Dictionary<IntPtr, INativePeripheralDelegate> s_nativePeripheralDelegateMap = new Dictionary<IntPtr, INativePeripheralDelegate>();

        public void Register(SafeNativePeripheralHandle handle, INativePeripheralDelegate peripheralDelegate)
        {
            s_nativePeripheralDelegateMap[handle.DangerousGetHandle()] = peripheralDelegate;
            NativeMethods.cb4u_peripheral_register_handlers(
                handle,
                DidDiscoverServices,
                DidDiscoverCharacteristics,
                DidUpdateValueForCharacteristic,
                DidWriteValueForCharacteristic,
                IsReadyToSendWriteWithoutResponse,
                DidUpdateNotificationStateForCharacteristic,
                DidReadRSSI,
                DidUpdateName,
                DidModifyServices
            );
        }

        public void Unregister(SafeNativePeripheralHandle handle)
        {
            s_nativePeripheralDelegateMap.Remove(handle.DangerousGetHandle());
        }

        static INativePeripheralDelegate GetDelegate(IntPtr peripheralPtr)
        {
            return s_nativePeripheralDelegateMap.GetValueOrDefault(peripheralPtr);
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralDidDiscoverServicesHandler))]
        public static void DidDiscoverServices(IntPtr peripheralPtr, IntPtr commaSeparatedServiceUUIDsPtr, int errorCode)
        {
            string commaSeparatedServiceUUIDs = Marshal.PtrToStringUTF8(commaSeparatedServiceUUIDsPtr);
            GetDelegate(peripheralPtr)?.DidDiscoverServices(
                commaSeparatedServiceUUIDs.Split(','),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralDidDiscoverCharacteristicsHandler))]
        public static void DidDiscoverCharacteristics(IntPtr peripheralPtr, IntPtr serviceUUIDPtr, IntPtr commaSeparatedCharacteristicUUIDsPtr, int errorCode)
        {
            string commaSeparatedCharacteristicUUIDs = Marshal.PtrToStringUTF8(commaSeparatedCharacteristicUUIDsPtr);
            GetDelegate(peripheralPtr)?.DidDiscoverCharacteristics(
                Marshal.PtrToStringUTF8(serviceUUIDPtr),
                commaSeparatedCharacteristicUUIDs.Split(','),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralDidUpdateValueForCharacteristicHandler))]
        public static void DidUpdateValueForCharacteristic(IntPtr peripheralPtr, IntPtr serviceUUIDPtr, IntPtr characteristicUUIDPtr, IntPtr dataPtr, int dataLength, int errorCode)
        {
            var dataBytes = new byte[dataLength];
            Marshal.Copy(dataPtr, dataBytes, 0, dataLength);

            GetDelegate(peripheralPtr)?.DidUpdateValueForCharacteristic(
                Marshal.PtrToStringUTF8(serviceUUIDPtr),
                Marshal.PtrToStringUTF8(characteristicUUIDPtr),
                dataBytes,
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralDidWriteValueForCharacteristicHandler))]
        public static void DidWriteValueForCharacteristic(IntPtr peripheralPtr, IntPtr serviceUUIDPtr, IntPtr characteristicUUIDPtr, int errorCode)
        {
            GetDelegate(peripheralPtr)?.DidWriteValueForCharacteristic(
                Marshal.PtrToStringUTF8(serviceUUIDPtr),
                Marshal.PtrToStringUTF8(characteristicUUIDPtr),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralIsReadyToSendWriteWithoutResponseHandler))]
        public static void IsReadyToSendWriteWithoutResponse(IntPtr peripheralPtr)
        {
            GetDelegate(peripheralPtr)?.IsReadyToSendWriteWithoutResponse();
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralDidUpdateNotificationStateForCharacteristicHandler))]
        public static void DidUpdateNotificationStateForCharacteristic(IntPtr peripheralPtr, IntPtr serviceUUIDPtr, IntPtr characteristicUUIDPtr, int notificationState, int errorCode)
        {
            GetDelegate(peripheralPtr)?.DidUpdateNotificationStateForCharacteristic(
                Marshal.PtrToStringUTF8(serviceUUIDPtr),
                Marshal.PtrToStringUTF8(characteristicUUIDPtr),
                notificationState == 1,
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralDidReadRSSIHandler))]
        public static void DidReadRSSI(IntPtr peripheralPtr, int rssi, int errorCode)
        {
            GetDelegate(peripheralPtr)?.DidReadRSSI(
                rssi,
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralDidUpdateNameHandler))]
        public static void DidUpdateName(IntPtr peripheralPtr)
        {
            GetDelegate(peripheralPtr)?.DidUpdateName();
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralDidModifyServicesHandler))]
        public static void DidModifyServices(IntPtr peripheralPtr, IntPtr commaSeparatedServiceUUIDsPtr)
        {
            string commaSeparatedServiceUUIDs = Marshal.PtrToStringUTF8(commaSeparatedServiceUUIDsPtr);
            GetDelegate(peripheralPtr)?.DidModifyServices(
                commaSeparatedServiceUUIDs.Split(',')
            );
        }
    }
}
