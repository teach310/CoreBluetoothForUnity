using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CoreBluetooth
{
    internal static class NativeMethods
    {
#if UNITY_IOS && !UNITY_EDITOR
        const string DLL_NAME = "__Internal";
#else
        const string DLL_NAME = "libCoreBluetoothForUnity";
#endif
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_central_release(IntPtr handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr cb4u_central_identifier(
            SafeNativeCentralHandle handle,
            [MarshalAs(UnmanagedType.LPStr), Out] StringBuilder identifier,
            int identifierSize
        );

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_central_maximum_update_value_length(SafeNativeCentralHandle handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern SafeNativeCentralManagerHandle cb4u_central_manager_new();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_central_manager_release(IntPtr handle);

        internal delegate void CB4UCentralManagerDidConnectHandler(IntPtr centralPtr, IntPtr peripheralIdPtr);
        internal delegate void CB4UCentralManagerDidDisconnectPeripheralHandler(IntPtr centralPtr, IntPtr peripheralIdPtr, int errorCode);
        internal delegate void CB4UCentralManagerDidFailToConnectHandler(IntPtr centralPtr, IntPtr peripheralIdPtr, int errorCode);
        internal delegate void CB4UCentralManagerDidDiscoverPeripheralHandler(IntPtr centralPtr, IntPtr peripheralPtr, int rssi);
        internal delegate void CB4UCentralManagerDidUpdateStateHandler(IntPtr centralPtr, CBManagerState state);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_central_manager_register_handlers(
            SafeNativeCentralManagerHandle handle,
            CB4UCentralManagerDidConnectHandler didConnectHandler,
            CB4UCentralManagerDidDisconnectPeripheralHandler didDisconnectPeripheralHandler,
            CB4UCentralManagerDidFailToConnectHandler didFailToConnectHandler,
            CB4UCentralManagerDidDiscoverPeripheralHandler didDiscoverPeripheralHandler,
            CB4UCentralManagerDidUpdateStateHandler didUpdateStateHandler
        );

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_central_manager_connect(SafeNativeCentralManagerHandle handle, SafeNativePeripheralHandle peripheralHandle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_central_manager_cancel_peripheral_connection(SafeNativeCentralManagerHandle handle, SafeNativePeripheralHandle peripheralHandle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_central_manager_scan_for_peripherals(
            SafeNativeCentralManagerHandle handle,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 2)] string[] serviceUUIDs,
            int serviceUUIDsCount
        );

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_central_manager_stop_scan(SafeNativeCentralManagerHandle handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool cb4u_central_manager_is_scanning(SafeNativeCentralManagerHandle handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_peripheral_release(IntPtr handle);

        // NOTE: using comma-separated service UUIDs instead of an array of service UUIDs to avoid marshalling issues
        internal delegate void CB4UPeripheralDidDiscoverServicesHandler(IntPtr peripheralPtr, IntPtr commaSeparatedServiceUUIDsPtr, int errorCode);
        internal delegate void CB4UPeripheralDidDiscoverCharacteristicsHandler(IntPtr peripheralPtr, IntPtr serviceUUIDPtr, IntPtr commaSeparatedCharacteristicUUIDsPtr, int errorCode);
        internal delegate void CB4UPeripheralDidUpdateValueForCharacteristicHandler(IntPtr peripheralPtr, IntPtr serviceUUIDPtr, IntPtr characteristicUUIDPtr, IntPtr dataPtr, int dataLength, int errorCode);
        internal delegate void CB4UPeripheralDidWriteValueForCharacteristicHandler(IntPtr peripheralPtr, IntPtr serviceUUIDPtr, IntPtr characteristicUUIDPtr, int errorCode);
        internal delegate void CB4UPeripheralDidUpdateNotificationStateForCharacteristicHandler(IntPtr peripheralPtr, IntPtr serviceUUIDPtr, IntPtr characteristicUUIDPtr, int notificationState, int errorCode);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_peripheral_register_handlers(
            SafeNativePeripheralHandle handle,
            CB4UPeripheralDidDiscoverServicesHandler didDiscoverServicesHandler,
            CB4UPeripheralDidDiscoverCharacteristicsHandler didDiscoverCharacteristicsHandler,
            CB4UPeripheralDidUpdateValueForCharacteristicHandler didUpdateValueForCharacteristicHandler,
            CB4UPeripheralDidWriteValueForCharacteristicHandler didWriteValueForCharacteristicHandler,
            CB4UPeripheralDidUpdateNotificationStateForCharacteristicHandler didUpdateNotificationStateForCharacteristicHandler
        );

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr cb4u_peripheral_identifier(
            SafeNativePeripheralHandle handle,
            [MarshalAs(UnmanagedType.LPStr), Out] StringBuilder identifier,
            int identifierSize
        );

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_peripheral_name(
            SafeNativePeripheralHandle handle,
            [MarshalAs(UnmanagedType.LPStr), Out] StringBuilder name,
            int nameSize
        );

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_peripheral_discover_services(
            SafeNativePeripheralHandle handle,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 2)] string[] serviceUUIDs,
            int serviceUUIDsCount
        );

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_peripheral_discover_characteristics(
            SafeNativePeripheralHandle handle,
            [MarshalAs(UnmanagedType.LPStr), In] string serviceUUID,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 3)] string[] characteristicUUIDs,
            int characteristicUUIDsCount
        );

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_peripheral_read_characteristic_value(
            SafeNativePeripheralHandle handle,
            [MarshalAs(UnmanagedType.LPStr), In] string serviceUUID,
            [MarshalAs(UnmanagedType.LPStr), In] string characteristicUUID
        );

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_peripheral_write_characteristic_value(
            SafeNativePeripheralHandle handle,
            [MarshalAs(UnmanagedType.LPStr), In] string serviceUUID,
            [MarshalAs(UnmanagedType.LPStr), In] string characteristicUUID,
            byte[] dataBytes,
            int dataLength,
            int writeType
        );

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_peripheral_set_notify_value(
            SafeNativePeripheralHandle handle,
            [MarshalAs(UnmanagedType.LPStr), In] string serviceUUID,
            [MarshalAs(UnmanagedType.LPStr), In] string characteristicUUID,
            [MarshalAs(UnmanagedType.I1)] bool enabled
        );

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_peripheral_state(SafeNativePeripheralHandle handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_peripheral_characteristic_properties(
            SafeNativePeripheralHandle handle,
            [MarshalAs(UnmanagedType.LPStr), In] string serviceUUID,
            [MarshalAs(UnmanagedType.LPStr), In] string characteristicUUID
        );

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern SafeNativePeripheralManagerHandle cb4u_peripheral_manager_new();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_peripheral_manager_release(IntPtr handle);

        internal delegate void CB4UPeripheralManagerDidUpdateStateHandler(IntPtr peripheralManagerPtr, CBManagerState state);
        internal delegate void CB4UPeripheralManagerDidAddServiceHandler(IntPtr peripheralManagerPtr, IntPtr serviceUUIDPtr, int errorCode);
        internal delegate void CB4UPeripheralManagerDidStartAdvertisingHandler(IntPtr peripheralManagerPtr, int errorCode);
        internal delegate void CB4UPeripheralManagerDidSubscribeToCharacteristicHandler(IntPtr peripheralManagerPtr, IntPtr centralPtr, IntPtr serviceUUIDPtr, IntPtr characteristicUUIDPtr);
        internal delegate void CB4UPeripheralManagerDidUnsubscribeFromCharacteristicHandler(IntPtr peripheralManagerPtr, IntPtr centralPtr, IntPtr serviceUUIDPtr, IntPtr characteristicUUIDPtr);
        internal delegate void CB4UPeripheralManagerIsReadyToUpdateSubscribersHandler(IntPtr peripheralManagerPtr);
        internal delegate void CB4UPeripheralManagerDidReceiveReadRequestHandler(IntPtr peripheralManagerPtr, IntPtr requestPtr);
        internal delegate void CB4UPeripheralManagerDidReceiveWriteRequestsHandler(IntPtr peripheralManagerPtr, IntPtr requestsPtr);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_peripheral_manager_register_handlers(
            SafeNativePeripheralManagerHandle handle,
            CB4UPeripheralManagerDidUpdateStateHandler didUpdateStateHandler,
            CB4UPeripheralManagerDidAddServiceHandler didAddServiceHandler,
            CB4UPeripheralManagerDidStartAdvertisingHandler didStartAdvertisingHandler,
            CB4UPeripheralManagerDidSubscribeToCharacteristicHandler didSubscribeToCharacteristicHandler,
            CB4UPeripheralManagerDidUnsubscribeFromCharacteristicHandler didUnsubscribeFromCharacteristicHandler,
            CB4UPeripheralManagerIsReadyToUpdateSubscribersHandler isReadyToUpdateSubscribersHandler,
            CB4UPeripheralManagerDidReceiveReadRequestHandler didReceiveReadRequestHandler,
            CB4UPeripheralManagerDidReceiveWriteRequestsHandler didReceiveWriteRequestsHandler
        );

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_peripheral_manager_add_service(
            SafeNativePeripheralManagerHandle peripheralHandle,
            SafeNativeMutableServiceHandle serviceHandle
        );

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_peripheral_manager_start_advertising(
            SafeNativePeripheralManagerHandle peripheralHandle,
            [MarshalAs(UnmanagedType.LPStr), In] string localName,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 3)] string[] serviceUUIDs,
            int serviceUUIDsCount
        );

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_peripheral_manager_stop_advertising(SafeNativePeripheralManagerHandle peripheralHandle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool cb4u_peripheral_manager_is_advertising(SafeNativePeripheralManagerHandle peripheralHandle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool cb4u_peripheral_manager_update_value(
            SafeNativePeripheralManagerHandle peripheralHandle,
            byte[] value,
            int valueLength,
            SafeNativeMutableCharacteristicHandle characteristicHandle,
            IntPtr[] subscribedCentrals,
            int subscribedCentralsCount
        );

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_peripheral_manager_respond_to_request(SafeNativePeripheralManagerHandle peripheralHandle, SafeNativeATTRequestHandle requestPtr, int errorCode);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern SafeNativeMutableServiceHandle cb4u_mutable_service_new([MarshalAs(UnmanagedType.LPStr), In] string uuid, [MarshalAs(UnmanagedType.I1)] bool primary);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_mutable_service_release(IntPtr handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_mutable_service_set_characteristics(SafeNativeMutableServiceHandle handle, IntPtr[] characteristics, int characteristicsCount);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern SafeNativeMutableCharacteristicHandle cb4u_mutable_characteristic_new(
            [MarshalAs(UnmanagedType.LPStr), In] string uuid,
            int properties,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 3)] byte[] dataBytes,
            int dataLength,
            int permissions
        );

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_mutable_characteristic_release(IntPtr handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_mutable_characteristic_value_length(SafeNativeMutableCharacteristicHandle handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_mutable_characteristic_value(SafeNativeMutableCharacteristicHandle handle, byte[] dataBytes, int dataLength);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_mutable_characteristic_set_value(SafeNativeMutableCharacteristicHandle handle, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 2), In] byte[] dataBytes, int dataLength);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_mutable_characteristic_properties(SafeNativeMutableCharacteristicHandle handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_mutable_characteristic_set_properties(SafeNativeMutableCharacteristicHandle handle, int properties);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_mutable_characteristic_permissions(SafeNativeMutableCharacteristicHandle handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_mutable_characteristic_set_permissions(SafeNativeMutableCharacteristicHandle handle, int permissions);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_att_request_release(IntPtr handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr cb4u_att_request_central(SafeNativeATTRequestHandle handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_att_request_characteristic_uuid(
            SafeNativeATTRequestHandle handle,
            [MarshalAs(UnmanagedType.LPStr), Out] StringBuilder serviceUUID,
            int serviceUUIDSize,
            [MarshalAs(UnmanagedType.LPStr), Out] StringBuilder characteristicUUID,
            int characteristicUUIDSize
        );

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_att_request_value_length(SafeNativeATTRequestHandle handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_att_request_value(SafeNativeATTRequestHandle handle, byte[] dataBytes, int dataLength);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cb4u_att_request_set_value(SafeNativeATTRequestHandle handle, byte[] dataBytes, int dataLength);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_att_request_offset(SafeNativeATTRequestHandle handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr cb4u_att_requests_release(IntPtr handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_att_requests_count(SafeNativeATTRequestsHandle handle);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr cb4u_att_requests_request(SafeNativeATTRequestsHandle handle, int index);
    }
}
