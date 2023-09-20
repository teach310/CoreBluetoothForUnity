using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreBluetooth
{
    internal class NativePeripheralProxy : INativePeripheral
    {
        string _peripheralId;
        readonly SafeNativeCentralManagerHandle _handle;

        internal NativePeripheralProxy(string peripheralId, SafeNativeCentralManagerHandle handle)
        {
            _peripheralId = peripheralId;
            _handle = handle;
        }

        void INativePeripheral.DiscoverServices(string[] serviceUUIDs)
        {
            foreach (string uuidString in serviceUUIDs)
            {
                ExceptionUtils.ThrowArgumentExceptionIfInvalidCBUUID(uuidString, nameof(serviceUUIDs));
            }

            int result = NativeMethods.cb4u_central_manager_peripheral_discover_services(
                _handle,
                _peripheralId,
                serviceUUIDs,
                serviceUUIDs?.Length ?? 0
            );

            if (result == -1)
            {
                throw new Exception($"Peripheral not found: {_peripheralId}");
            }
        }

        CBPeripheralState INativePeripheral.State
        {
            get
            {
                int state = NativeMethods.cb4u_central_manager_peripheral_state(_handle, _peripheralId);
                return (CBPeripheralState)state;
            }
        }
    }
}
