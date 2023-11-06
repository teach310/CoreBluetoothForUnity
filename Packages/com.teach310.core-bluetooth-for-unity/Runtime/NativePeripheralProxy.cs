using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CoreBluetooth
{
    internal interface INativePeripheralCallbacks
    {
        void Register(SafeNativePeripheralHandle handle, INativePeripheralDelegate peripheralDelegate);
        void Unregister(SafeNativePeripheralHandle handle);
    }

    internal class NativePeripheralProxy : IDisposable
    {
        readonly SafeNativePeripheralHandle _handle;
        readonly INativePeripheralCallbacks _callbacks;

        public NativePeripheralProxy(SafeNativePeripheralHandle handle, INativePeripheralDelegate peripheralDelegate)
        {
            _handle = handle;
            _callbacks = ServiceLocator.Instance.Resolve<INativePeripheralCallbacks>();
            if (peripheralDelegate != null)
            {
                _callbacks.Register(handle, peripheralDelegate);
            }
        }

        public string Identifier
        {
            get
            {
                var sb = new StringBuilder(64);
                NativeMethods.cb4u_peripheral_identifier(_handle, sb, sb.Capacity);
                return sb.ToString();
            }
        }

        public string Name
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

        public void DiscoverServices(string[] serviceUUIDs)
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

        public void DiscoverCharacteristics(string[] characteristicUUIDs, CBService service)
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

        public void ReadValue(CBCharacteristic characteristic)
        {
            int result = NativeMethods.cb4u_peripheral_read_characteristic_value(
                _handle,
                characteristic.Service.UUID,
                characteristic.UUID
            );

            ExceptionUtils.ThrowIfServiceNotFound(result, characteristic.Service.UUID);
            ExceptionUtils.ThrowIfCharacteristicNotFound(result, characteristic.UUID);
        }

        public void WriteValue(byte[] data, CBCharacteristic characteristic, CBCharacteristicWriteType writeType)
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

        public int GetMaximumWriteValueLength(CBCharacteristicWriteType writeType)
        {
            return NativeMethods.cb4u_peripheral_maximum_write_value_length(_handle, (int)writeType);
        }

        public void SetNotifyValue(bool enabled, CBCharacteristic characteristic)
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

        public CBPeripheralState State
        {
            get
            {
                int state = NativeMethods.cb4u_peripheral_state(_handle);
                return (CBPeripheralState)state;
            }
        }

        public bool CanSendWriteWithoutResponse
        {
            get
            {
                return NativeMethods.cb4u_peripheral_can_send_write_without_response(_handle);
            }
        }

        public void ReadRSSI()
        {
            NativeMethods.cb4u_peripheral_read_rssi(_handle);
        }

        public void Dispose()
        {
            _callbacks.Unregister(_handle);
        }
    }
}
