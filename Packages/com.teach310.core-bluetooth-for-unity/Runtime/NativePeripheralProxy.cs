using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CoreBluetooth
{
    internal class NativePeripheralProxy : IDisposable
    {
        readonly static Dictionary<IntPtr, INativePeripheralDelegate> s_nativePeripheralDelegateMap = new Dictionary<IntPtr, INativePeripheralDelegate>();

        readonly SafeNativePeripheralHandle _handle;

        public NativePeripheralProxy(SafeNativePeripheralHandle handle, INativePeripheralDelegate peripheralDelegate)
        {
            _handle = handle;
            if (peripheralDelegate != null)
            {
                s_nativePeripheralDelegateMap[handle.DangerousGetHandle()] = peripheralDelegate;
            }
            RegisterHandlers();
        }

        public string Identifier
        {
            get
            {
                var sb = new StringBuilder(64);
                NativeMethods.cb4u_peripheral_identifier(_handle, sb, sb.Capacity);
                return sb.ToString();
            }
        }

        public string Name
        {
            get
            {
                var sb = new StringBuilder(64);
                int result = NativeMethods.cb4u_peripheral_name(_handle, sb, sb.Capacity);

                if (result == 0)
                {
                    return null;
                }
                return sb.ToString();
            }
        }

        public void DiscoverServices(string[] serviceUUIDs)
        {
            if (serviceUUIDs != null)
            {
                foreach (string uuidString in serviceUUIDs)
                {
                    ExceptionUtils.ThrowArgumentExceptionIfInvalidCBUUID(uuidString, nameof(serviceUUIDs));
                }
            }

            NativeMethods.cb4u_peripheral_discover_services(
                _handle,
                serviceUUIDs,
                serviceUUIDs?.Length ?? 0
            );
        }

        public void DiscoverCharacteristics(string[] characteristicUUIDs, CBService service)
        {
            if (characteristicUUIDs != null)
            {
                foreach (string uuidString in characteristicUUIDs)
                {
                    ExceptionUtils.ThrowArgumentExceptionIfInvalidCBUUID(uuidString, nameof(characteristicUUIDs));
                }
            }

            int result = NativeMethods.cb4u_peripheral_discover_characteristics(
                _handle,
                service.UUID,
                characteristicUUIDs,
                characteristicUUIDs?.Length ?? 0
            );

            ExceptionUtils.ThrowIfServiceNotFound(result, service.UUID);
        }

        public void ReadValue(CBCharacteristic characteristic)
        {
            int result = NativeMethods.cb4u_peripheral_read_characteristic_value(
                _handle,
                characteristic.Service.UUID,
                characteristic.UUID
            );

            ExceptionUtils.ThrowIfServiceNotFound(result, characteristic.Service.UUID);
            ExceptionUtils.ThrowIfCharacteristicNotFound(result, characteristic.UUID);
        }

        public void WriteValue(byte[] data, CBCharacteristic characteristic, CBCharacteristicWriteType writeType)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));


            int result = NativeMethods.cb4u_peripheral_write_characteristic_value(
                _handle,
                characteristic.Service.UUID,
                characteristic.UUID,
                data,
                data.Length,
                (int)writeType
            );

            ExceptionUtils.ThrowIfServiceNotFound(result, characteristic.Service.UUID);
            ExceptionUtils.ThrowIfCharacteristicNotFound(result, characteristic.UUID);
        }

        public int GetMaximumWriteValueLength(CBCharacteristicWriteType writeType)
        {
            return NativeMethods.cb4u_peripheral_maximum_write_value_length(_handle, (int)writeType);
        }

        public void SetNotifyValue(bool enabled, CBCharacteristic characteristic)
        {
            int result = NativeMethods.cb4u_peripheral_set_notify_value(
                _handle,
                characteristic.Service.UUID,
                characteristic.UUID,
                enabled
            );

            ExceptionUtils.ThrowIfServiceNotFound(result, characteristic.Service.UUID);
            ExceptionUtils.ThrowIfCharacteristicNotFound(result, characteristic.UUID);
        }

        public CBPeripheralState State
        {
            get
            {
                int state = NativeMethods.cb4u_peripheral_state(_handle);
                return (CBPeripheralState)state;
            }
        }

        public bool CanSendWriteWithoutResponse
        {
            get
            {
                return NativeMethods.cb4u_peripheral_can_send_write_without_response(_handle);
            }
        }

        public void ReadRSSI()
        {
            NativeMethods.cb4u_peripheral_read_rssi(_handle);
        }

        void RegisterHandlers()
        {
            NativeMethods.cb4u_peripheral_register_handlers(
                _handle,
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

        public void Dispose()
        {
            s_nativePeripheralDelegateMap.Remove(_handle.DangerousGetHandle());
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
