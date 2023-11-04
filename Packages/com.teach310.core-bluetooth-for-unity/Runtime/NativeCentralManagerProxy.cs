using System;
using System.Linq;

namespace CoreBluetooth
{
    internal interface INativeCentralManagerCallbacks
    {
        void Register(SafeNativeCentralManagerHandle centralPtr, INativeCentralManagerDelegate centralManagerDelegate);
        void Unregister(SafeNativeCentralManagerHandle centralPtr);
    }

    internal class NativeCentralManagerProxy : IDisposable
    {
        readonly SafeNativeCentralManagerHandle _handle;
        readonly INativeCentralManagerCallbacks _callbacks;

        public NativeCentralManagerProxy(SafeNativeCentralManagerHandle handle, INativeCentralManagerDelegate centralManagerDelegate)
        {
            _handle = handle;
            _callbacks = ServiceLocator.Instance.Resolve<INativeCentralManagerCallbacks>();
            if (centralManagerDelegate != null)
            {
                _callbacks.Register(handle, centralManagerDelegate);
            }
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

        public void Dispose()
        {
            _callbacks.Unregister(_handle);
        }
    }
}
