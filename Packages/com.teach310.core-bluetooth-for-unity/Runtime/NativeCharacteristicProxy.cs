using System;

namespace CoreBluetooth
{
    public class NativeCharacteristicProxy : INativeCharacteristic
    {
        string _peripheralId;
        string _serviceUUID;
        string _characteristicUUID;
        readonly SafeNativeCentralManagerHandle _handle;

        internal NativeCharacteristicProxy(string peripheralId, string serviceUUID, string characteristicUUID, SafeNativeCentralManagerHandle handle)
        {
            _peripheralId = peripheralId;
            _serviceUUID = serviceUUID;
            _characteristicUUID = characteristicUUID;
            _handle = handle;
        }

        CBCharacteristicProperties INativeCharacteristic.Properties
        {
            get
            {
                int result = NativeMethods.cb4u_central_manager_characteristic_properties(
                    _handle,
                    _peripheralId,
                    _serviceUUID,
                    _characteristicUUID
                );

                ExceptionUtils.ThrowIfPeripheralNotFound(result, _peripheralId);
                ExceptionUtils.ThrowIfServiceNotFound(result, _serviceUUID);
                ExceptionUtils.ThrowIfCharacteristicNotFound(result, _characteristicUUID);

                return (CBCharacteristicProperties)result;
            }
        }
    }
}
