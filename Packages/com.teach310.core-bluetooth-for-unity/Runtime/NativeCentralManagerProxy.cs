using System;

namespace CoreBluetooth
{
    internal class NativeCentralManagerProxy
    {
        readonly SafeNativeCentralManagerHandle _handle;

        internal NativeCentralManagerProxy(SafeNativeCentralManagerHandle handle)
        {
            _handle = handle;
        }

        internal void ScanForPeripherals(string[] serviceUUIDs = null)
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

        internal void StopScan()
        {
            NativeMethods.cb4u_central_manager_stop_scan(_handle);
        }

        internal bool IsScanning()
        {
            return NativeMethods.cb4u_central_manager_is_scanning(_handle);
        }
    }
}
