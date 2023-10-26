using System;

namespace CoreBluetooth
{
    internal class NativeCentralManagerProxy
    {
        readonly SafeNativeCentralManagerHandle _handle;

        public NativeCentralManagerProxy(SafeNativeCentralManagerHandle handle, INativeCentralManagerDelegate centralManagerDelegate)
        {
            _handle = handle;
            _handle.SetDelegate(centralManagerDelegate);
        }

        public void Connect(CBPeripheral peripheral)
        {
            NativeMethods.cb4u_central_manager_connect(_handle, peripheral.Handle);
        }

        public void CancelPeripheralConnection(CBPeripheral peripheral)
        {
            NativeMethods.cb4u_central_manager_cancel_peripheral_connection(_handle, peripheral.Handle);
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
    }
}
