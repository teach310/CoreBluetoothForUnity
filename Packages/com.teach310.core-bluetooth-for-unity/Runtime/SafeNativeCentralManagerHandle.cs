using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    internal class SafeNativeCentralManagerHandle : SafeHandle
    {
        static Dictionary<IntPtr, CBCentralManager> s_centralManagerMap = new Dictionary<IntPtr, CBCentralManager>();

        public override bool IsInvalid => handle == IntPtr.Zero;

        SafeNativeCentralManagerHandle(IntPtr handle) : base(handle, true) { }

        internal static SafeNativeCentralManagerHandle Create(CBCentralManager centralManager)
        {
            var handle = NativeMethods.cb4u_central_manager_new();
            var instance = new SafeNativeCentralManagerHandle(handle);
            RegisterHandlers(instance);
            s_centralManagerMap.Add(handle, centralManager);
            return instance;
        }

        protected override bool ReleaseHandle()
        {
            s_centralManagerMap.Remove(handle);
            NativeMethods.cb4u_central_manager_release(handle);
            return true;
        }

        static void RegisterHandlers(SafeNativeCentralManagerHandle handle)
        {
            NativeMethods.cb4u_central_manager_register_handlers(
                handle,
                DidConnect,
                DidDisconnectPeripheral,
                DidFailToConnect,
                DidDiscoverPeripheral,
                DidUpdateState,
                PeripheralDidDiscoverServices,
                PeripheralDidDiscoverCharacteristics,
                PeripheralDidUpdateValueForCharacteristic,
                PeripheralDidWriteValueForCharacteristic,
                PeripheralDidUpdateNotificationStateForCharacteristic
            );
        }

        static CBCentralManager GetCentralManager(IntPtr centralPtr)
        {
            if (!s_centralManagerMap.TryGetValue(centralPtr, out var centralManager))
            {
                UnityEngine.Debug.LogError("CBCentralManager instance not found.");
            }
            return centralManager;
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidConnectHandler))]
        internal static void DidConnect(IntPtr centralPtr, IntPtr peripheralIdPtr)
        {
            GetCentralManager(centralPtr)?.DidConnect(Marshal.PtrToStringUTF8(peripheralIdPtr));
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidDisconnectPeripheralHandler))]
        internal static void DidDisconnectPeripheral(IntPtr centralPtr, IntPtr peripheralIdPtr, int errorCode)
        {
            GetCentralManager(centralPtr)?.DidDisconnectPeripheral(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidFailToConnectHandler))]
        internal static void DidFailToConnect(IntPtr centralPtr, IntPtr peripheralIdPtr, int errorCode)
        {
            GetCentralManager(centralPtr)?.DidFailToConnect(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidDiscoverPeripheralHandler))]
        internal static void DidDiscoverPeripheral(IntPtr centralPtr, IntPtr peripheralIdPtr, IntPtr peripheralNamePtr, int rssi)
        {
            GetCentralManager(centralPtr)?.DidDiscoverPeripheral(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                Marshal.PtrToStringUTF8(peripheralNamePtr),
                rssi
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidUpdateStateHandler))]
        internal static void DidUpdateState(IntPtr centralPtr, CBManagerState state)
        {
            GetCentralManager(centralPtr)?.DidUpdateState(state);
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralDidDiscoverServicesHandler))]
        internal static void PeripheralDidDiscoverServices(IntPtr centralPtr, IntPtr peripheralIdPtr, IntPtr commaSeparatedServiceUUIDsPtr, int errorCode)
        {
            string commaSeparatedServiceUUIDs = Marshal.PtrToStringUTF8(commaSeparatedServiceUUIDsPtr);
            if (string.IsNullOrEmpty(commaSeparatedServiceUUIDs))
            {
                throw new ArgumentException("commaSeparatedServiceUUIDs is null or empty.");
            }

            GetCentralManager(centralPtr)?.PeripheralDidDiscoverServices(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                commaSeparatedServiceUUIDs.Split(','),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralDidDiscoverCharacteristicsHandler))]
        internal static void PeripheralDidDiscoverCharacteristics(IntPtr centralPtr, IntPtr peripheralIdPtr, IntPtr serviceUUIDPtr, IntPtr commaSeparatedCharacteristicUUIDsPtr, int errorCode)
        {
            string commaSeparatedCharacteristicUUIDs = Marshal.PtrToStringUTF8(commaSeparatedCharacteristicUUIDsPtr);
            if (string.IsNullOrEmpty(commaSeparatedCharacteristicUUIDs))
            {
                throw new ArgumentException("commaSeparatedCharacteristicUUIDs is null or empty.");
            }

            GetCentralManager(centralPtr)?.PeripheralDidDiscoverCharacteristics(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                Marshal.PtrToStringUTF8(serviceUUIDPtr),
                commaSeparatedCharacteristicUUIDs.Split(','),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralDidUpdateValueForCharacteristicHandler))]
        internal static void PeripheralDidUpdateValueForCharacteristic(IntPtr centralPtr, IntPtr peripheralIdPtr, IntPtr serviceUUIDPtr, IntPtr characteristicUUIDPtr, IntPtr dataPtr, int dataLength, int errorCode)
        {
            var dataBytes = new byte[dataLength];
            Marshal.Copy(dataPtr, dataBytes, 0, dataLength);

            GetCentralManager(centralPtr)?.PeripheralDidUpdateValueForCharacteristic(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                Marshal.PtrToStringUTF8(serviceUUIDPtr),
                Marshal.PtrToStringUTF8(characteristicUUIDPtr),
                dataBytes,
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralDidWriteValueForCharacteristicHandler))]
        internal static void PeripheralDidWriteValueForCharacteristic(IntPtr centralPtr, IntPtr peripheralIdPtr, IntPtr serviceUUIDPtr, IntPtr characteristicUUIDPtr, int errorCode)
        {
            GetCentralManager(centralPtr)?.PeripheralDidWriteValueForCharacteristic(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                Marshal.PtrToStringUTF8(serviceUUIDPtr),
                Marshal.PtrToStringUTF8(characteristicUUIDPtr),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralDidUpdateNotificationStateForCharacteristicHandler))]
        internal static void PeripheralDidUpdateNotificationStateForCharacteristic(IntPtr centralPtr, IntPtr peripheralIdPtr, IntPtr serviceUUIDPtr, IntPtr characteristicUUIDPtr, int notificationState, int errorCode)
        {
            GetCentralManager(centralPtr)?.PeripheralDidUpdateNotificationStateForCharacteristic(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                Marshal.PtrToStringUTF8(serviceUUIDPtr),
                Marshal.PtrToStringUTF8(characteristicUUIDPtr),
                notificationState == 1,
                CBError.CreateOrNullFromCode(errorCode)
            );
        }
    }
}
