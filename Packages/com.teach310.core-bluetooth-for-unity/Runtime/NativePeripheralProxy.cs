using System;

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
            if (serviceUUIDs != null)
            {
                foreach (string uuidString in serviceUUIDs)
                {
                    ExceptionUtils.ThrowArgumentExceptionIfInvalidCBUUID(uuidString, nameof(serviceUUIDs));
                }
            }

            int result = NativeMethods.cb4u_central_manager_peripheral_discover_services(
                _handle,
                _peripheralId,
                serviceUUIDs,
                serviceUUIDs?.Length ?? 0
            );
            ExceptionUtils.ThrowIfPeripheralNotFound(result, _peripheralId);
        }

        void INativePeripheral.DiscoverCharacteristics(string[] characteristicUUIDs, CBService service)
        {
            if (characteristicUUIDs != null)
            {
                foreach (string uuidString in characteristicUUIDs)
                {
                    ExceptionUtils.ThrowArgumentExceptionIfInvalidCBUUID(uuidString, nameof(characteristicUUIDs));
                }
            }

            int result = NativeMethods.cb4u_central_manager_peripheral_discover_characteristics(
                _handle,
                _peripheralId,
                service.UUID,
                characteristicUUIDs,
                characteristicUUIDs?.Length ?? 0
            );

            ExceptionUtils.ThrowIfPeripheralNotFound(result, _peripheralId);
            ExceptionUtils.ThrowIfServiceNotFound(result, service.UUID);
        }

        void INativePeripheral.ReadValue(CBCharacteristic characteristic)
        {
            int result = NativeMethods.cb4u_central_manager_peripheral_read_characteristic_value(
                _handle,
                _peripheralId,
                characteristic.Service.UUID,
                characteristic.UUID
            );

            ExceptionUtils.ThrowIfPeripheralNotFound(result, _peripheralId);
            ExceptionUtils.ThrowIfServiceNotFound(result, characteristic.Service.UUID);
            ExceptionUtils.ThrowIfCharacteristicNotFound(result, characteristic.UUID);
        }

        void INativePeripheral.WriteValue(byte[] data, CBCharacteristic characteristic, CBCharacteristicWriteType writeType)
        {
            int result = NativeMethods.cb4u_central_manager_peripheral_write_characteristic_value(
                _handle,
                _peripheralId,
                characteristic.Service.UUID,
                characteristic.UUID,
                data,
                data.Length,
                (int)writeType
            );

            ExceptionUtils.ThrowIfPeripheralNotFound(result, _peripheralId);
            ExceptionUtils.ThrowIfServiceNotFound(result, characteristic.Service.UUID);
            ExceptionUtils.ThrowIfCharacteristicNotFound(result, characteristic.UUID);
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
