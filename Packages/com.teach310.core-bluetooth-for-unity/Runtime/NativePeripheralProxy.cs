using System;
using System.Text;

namespace CoreBluetooth
{
    internal class NativePeripheralProxy
    {
        readonly SafeNativePeripheralHandle _handle;

        internal NativePeripheralProxy(SafeNativePeripheralHandle handle, INativePeripheralDelegate peripheralDelegate)
        {
            _handle = handle;
            _handle.SetDelegate(peripheralDelegate);
        }

        internal string Identifier
        {
            get
            {
                var sb = new StringBuilder(64);
                NativeMethods.cb4u_peripheral_identifier(_handle, sb, sb.Capacity);
                return sb.ToString();
            }
        }

        internal string Name
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

        internal void DiscoverServices(string[] serviceUUIDs)
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

        internal void DiscoverCharacteristics(string[] characteristicUUIDs, CBService service)
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

        internal void ReadValue(CBCharacteristic characteristic)
        {
            int result = NativeMethods.cb4u_peripheral_read_characteristic_value(
                _handle,
                characteristic.Service.UUID,
                characteristic.UUID
            );

            ExceptionUtils.ThrowIfServiceNotFound(result, characteristic.Service.UUID);
            ExceptionUtils.ThrowIfCharacteristicNotFound(result, characteristic.UUID);
        }

        internal void WriteValue(byte[] data, CBCharacteristic characteristic, CBCharacteristicWriteType writeType)
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

        internal int GetMaximumWriteValueLength(CBCharacteristicWriteType writeType)
        {
            return NativeMethods.cb4u_peripheral_maximum_write_value_length(_handle, (int)writeType);
        }

        internal void SetNotifyValue(bool enabled, CBCharacteristic characteristic)
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

        internal CBPeripheralState State
        {
            get
            {
                int state = NativeMethods.cb4u_peripheral_state(_handle);
                return (CBPeripheralState)state;
            }
        }

        internal bool CanSendWriteWithoutResponse
        {
            get
            {
                return NativeMethods.cb4u_peripheral_can_send_write_without_response(_handle);
            }
        }
    }
}
