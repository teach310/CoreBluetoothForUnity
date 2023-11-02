using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    internal class NativeCentralManagerProxy : IDisposable
    {
        readonly static Dictionary<IntPtr, INativeCentralManagerDelegate> s_centralManagerDelegateMap = new Dictionary<IntPtr, INativeCentralManagerDelegate>();

        readonly SafeNativeCentralManagerHandle _handle;

        public NativeCentralManagerProxy(SafeNativeCentralManagerHandle handle, INativeCentralManagerDelegate centralManagerDelegate)
        {
            _handle = handle;
            if (centralManagerDelegate != null)
            {
                s_centralManagerDelegateMap[handle.DangerousGetHandle()] = centralManagerDelegate;
            }
            RegisterHandlers();
        }

        public void Connect(CBPeripheral peripheral)
        {
            NativeMethods.cb4u_central_manager_connect(_handle, peripheral.Handle);
        }

        public void CancelPeripheralConnection(CBPeripheral peripheral)
        {
            NativeMethods.cb4u_central_manager_cancel_peripheral_connection(_handle, peripheral.Handle);
        }

        public SafeNativePeripheralHandle[] RetrievePeripherals(string[] peripheralIds)
        {
            var arrayHandle = NativeMethods.cb4u_central_manager_retrieve_peripherals(_handle, peripheralIds, peripheralIds.Length);
            var peripheralPtrs = Foundation.NSArray.ArrayFromHandle<SafeNativePeripheralHandle>(arrayHandle);
            return peripheralPtrs.Select(ptr => new SafeNativePeripheralHandle(ptr)).ToArray();
        }

        public void ScanForPeripherals(string[] serviceUUIDs = null)
        {
            foreach (string uuidString in serviceUUIDs)
            {
                ExceptionUtils.ThrowArgumentExceptionIfInvalidCBUUID(uuidString, nameof(serviceUUIDs));
            }

            NativeMethods.cb4u_central_manager_scan_for_peripherals(
                _handle,
                serviceUUIDs,
                serviceUUIDs?.Length ?? 0
            );
        }

        public void StopScan()
        {
            NativeMethods.cb4u_central_manager_stop_scan(_handle);
        }

        public bool IsScanning()
        {
            return NativeMethods.cb4u_central_manager_is_scanning(_handle);
        }

        void RegisterHandlers()
        {
            NativeMethods.cb4u_central_manager_register_handlers(
                _handle,
                DidConnect,
                DidDisconnectPeripheral,
                DidFailToConnect,
                DidDiscoverPeripheral,
                DidUpdateState
            );
        }

        public void Dispose()
        {
            s_centralManagerDelegateMap.Remove(_handle.DangerousGetHandle());
        }

        static INativeCentralManagerDelegate GetDelegate(IntPtr centralPtr)
        {
            return s_centralManagerDelegateMap.GetValueOrDefault(centralPtr);
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidConnectHandler))]
        internal static void DidConnect(IntPtr centralPtr, IntPtr peripheralIdPtr)
        {
            GetDelegate(centralPtr)?.DidConnect(Marshal.PtrToStringUTF8(peripheralIdPtr));
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidDisconnectPeripheralHandler))]
        public static void DidDisconnectPeripheral(IntPtr centralPtr, IntPtr peripheralIdPtr, int errorCode)
        {
            GetDelegate(centralPtr)?.DidDisconnectPeripheral(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidFailToConnectHandler))]
        public static void DidFailToConnect(IntPtr centralPtr, IntPtr peripheralIdPtr, int errorCode)
        {
            GetDelegate(centralPtr)?.DidFailToConnect(
                Marshal.PtrToStringUTF8(peripheralIdPtr),
                CBError.CreateOrNullFromCode(errorCode)
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidDiscoverPeripheralHandler))]
        public static void DidDiscoverPeripheral(IntPtr centralPtr, IntPtr peripheralPtr, int rssi)
        {
            GetDelegate(centralPtr)?.DidDiscoverPeripheral(
                new SafeNativePeripheralHandle(peripheralPtr),
                rssi
            );
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidUpdateStateHandler))]
        public static void DidUpdateState(IntPtr centralPtr, CBManagerState state)
        {
            GetDelegate(centralPtr)?.DidUpdateState(state);
        }
    }
}
