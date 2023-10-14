using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    internal interface INativePeripheralDelegate
    {
        void DidDiscoverServices(string[] serviceUUIDs, CBError error) { }
        void DidDiscoverCharacteristics(string serviceUUID, string[] characteristicUUIDs, CBError error) { }
        void DidUpdateValueForCharacteristic(string serviceUUID, string characteristicUUID, byte[] data, CBError error) { }
        void DidWriteValueForCharacteristic(string serviceUUID, string characteristicUUID, CBError error) { }
        void DidUpdateNotificationStateForCharacteristic(string serviceUUID, string characteristicUUID, bool enabled, CBError error) { }
    }

    internal class SafeNativePeripheralHandle : SafeHandle
    {
        static Dictionary<IntPtr, INativePeripheralDelegate> s_nativePeripheralDelegateMap = new Dictionary<IntPtr, INativePeripheralDelegate>();

        public override bool IsInvalid => handle == IntPtr.Zero;

        internal SafeNativePeripheralHandle(IntPtr handle) : base(handle, true)
        {
            RegisterHandlers();
        }

        void RegisterHandlers()
        {
            NativeMethods.cb4u_peripheral_register_handlers(
                this,
                DidDiscoverServices,
                DidDiscoverCharacteristics,
                DidUpdateValueForCharacteristic,
                DidWriteValueForCharacteristic,
                DidUpdateNotificationStateForCharacteristic
            );
        }

        internal void SetDelegate(INativePeripheralDelegate peripheralDelegate)
        {
            s_nativePeripheralDelegateMap[handle] = peripheralDelegate;
        }

        protected override bool ReleaseHandle()
        {
            s_nativePeripheralDelegateMap.Remove(handle);
            NativeMethods.cb4u_peripheral_release(handle);
            return true;
        }

        static INativePeripheralDelegate GetDelegate(IntPtr peripheralPtr)
        {
            if (!s_nativePeripheralDelegateMap.TryGetValue(peripheralPtr, out var peripheralDelegate))
            {
                return null;
            }
            return peripheralDelegate;
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralDidDiscoverServicesHandler))]
        internal static void DidDiscoverServices(IntPtr peripheralPtr, IntPtr commaSeparatedServiceUUIDsPtr, int errorCode)
        {
            string commaSeparatedServiceUUIDs = Marshal.PtrToStringUTF8(commaSeparatedServiceUUIDsPtr);
            if (string.IsNullOrEmpty(commaSeparatedServiceUUIDs))
            {
                throw new ArgumentException("commaSeparatedServiceUUIDs is null or empty.");
            }

            GetDelegate(peripheralPtr)?.DidDiscoverServices(
                commaSeparatedServiceUUIDs.Split(','),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralDidDiscoverCharacteristicsHandler))]
        internal static void DidDiscoverCharacteristics(IntPtr peripheralPtr, IntPtr serviceUUIDPtr, IntPtr commaSeparatedCharacteristicUUIDsPtr, int errorCode)
        {
            string commaSeparatedCharacteristicUUIDs = Marshal.PtrToStringUTF8(commaSeparatedCharacteristicUUIDsPtr);
            if (string.IsNullOrEmpty(commaSeparatedCharacteristicUUIDs))
            {
                throw new ArgumentException("commaSeparatedCharacteristicUUIDs is null or empty.");
            }

            GetDelegate(peripheralPtr)?.DidDiscoverCharacteristics(
                Marshal.PtrToStringUTF8(serviceUUIDPtr),
                commaSeparatedCharacteristicUUIDs.Split(','),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralDidUpdateValueForCharacteristicHandler))]
        internal static void DidUpdateValueForCharacteristic(IntPtr peripheralPtr, IntPtr serviceUUIDPtr, IntPtr characteristicUUIDPtr, IntPtr dataPtr, int dataLength, int errorCode)
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
        internal static void DidWriteValueForCharacteristic(IntPtr peripheralPtr, IntPtr serviceUUIDPtr, IntPtr characteristicUUIDPtr, int errorCode)
        {
            GetDelegate(peripheralPtr)?.DidWriteValueForCharacteristic(
                Marshal.PtrToStringUTF8(serviceUUIDPtr),
                Marshal.PtrToStringUTF8(characteristicUUIDPtr),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralDidUpdateNotificationStateForCharacteristicHandler))]
        internal static void DidUpdateNotificationStateForCharacteristic(IntPtr peripheralPtr, IntPtr serviceUUIDPtr, IntPtr characteristicUUIDPtr, int notificationState, int errorCode)
        {
            GetDelegate(peripheralPtr)?.DidUpdateNotificationStateForCharacteristic(
                Marshal.PtrToStringUTF8(serviceUUIDPtr),
                Marshal.PtrToStringUTF8(characteristicUUIDPtr),
                notificationState == 1,
                CBError.CreateOrNullFromCode(errorCode)
            );
        }
    }
}
