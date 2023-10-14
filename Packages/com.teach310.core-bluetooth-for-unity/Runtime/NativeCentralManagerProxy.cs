using System;

namespace CoreBluetooth
{
    internal class NativeCentralManagerProxy
    {
        readonly SafeNativeCentralManagerHandle _handle;

        internal NativeCentralManagerProxy(SafeNativeCentralManagerHandle handle, INativeCentralManagerDelegate centralManagerDelegate)
        {
            _handle = handle;
            _handle.SetDelegate(centralManagerDelegate);
        }

        internal void Connect(CBPeripheral peripheral)
        {
            NativeMethods.cb4u_central_manager_connect(_handle, peripheral.Handle);
        }

        internal void CancelPeripheralConnection(CBPeripheral peripheral)
        {
            NativeMethods.cb4u_central_manager_cancel_peripheral_connection(_handle, peripheral.Handle);
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
