using System;

namespace CoreBluetooth
{
    internal class NativeCharacteristicProxy : INativeCharacteristic
    {
        string _serviceUUID;
        string _characteristicUUID;
        readonly SafeNativePeripheralHandle _handle;

        internal NativeCharacteristicProxy(string serviceUUID, string characteristicUUID, SafeNativePeripheralHandle handle)
        {
            _serviceUUID = serviceUUID;
            _characteristicUUID = characteristicUUID;
            _handle = handle;
        }

        CBCharacteristicProperties INativeCharacteristic.Properties
        {
            get
            {
                int result = NativeMethods.cb4u_peripheral_characteristic_properties(_handle, _serviceUUID, _characteristicUUID);

                ExceptionUtils.ThrowIfServiceNotFound(result, _serviceUUID);
                ExceptionUtils.ThrowIfCharacteristicNotFound(result, _characteristicUUID);

                return (CBCharacteristicProperties)result;
            }
        }
    }
}
