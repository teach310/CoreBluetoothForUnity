using System.Text;

namespace CoreBluetooth
{
    internal class NativeATTRequestProxy : INativeATTRequest
    {
        SafeNativeATTRequestHandle _handle;
        IPeripheralManagerData _peripheralManagerData;

        internal NativeATTRequestProxy(SafeNativeATTRequestHandle handle, IPeripheralManagerData peripheralManagerData)
        {
            _handle = handle;
            _peripheralManagerData = peripheralManagerData;
        }

        CBCentral INativeATTRequest.Central
        {
            get
            {
                var central = NewCentral();
                var foundCentral = _peripheralManagerData.FindCentral(central.Identifier);
                if (foundCentral == null)
                {
                    _peripheralManagerData.AddCentral(central);
                    foundCentral = central;
                }
                else
                {
                    central.Dispose();
                }
                return foundCentral;
            }
        }

        CBCentral NewCentral()
        {
            var centralPtr = NativeMethods.cb4u_att_request_central(_handle);
            var centralHandle = new SafeNativeCentralHandle(centralPtr);
            return new CBCentral(centralHandle);
        }

        CBCharacteristic INativeATTRequest.Characteristic
        {
            get
            {
                var serviceUUIDBuilder = new StringBuilder(64);
                var characteristicUUIDBuilder = new StringBuilder(64);
                NativeMethods.cb4u_att_request_characteristic_uuid(
                    _handle,
                    serviceUUIDBuilder,
                    serviceUUIDBuilder.Capacity,
                    characteristicUUIDBuilder,
                    characteristicUUIDBuilder.Capacity
                );
                return _peripheralManagerData.FindCharacteristic(serviceUUIDBuilder.ToString(), characteristicUUIDBuilder.ToString());
            }
        }

        void INativeATTRequest.SetValue(byte[] value)
        {
            NativeMethods.cb4u_att_request_set_value(_handle, value, value?.Length ?? 0);
        }

        int INativeATTRequest.Offset => NativeMethods.cb4u_att_request_offset(_handle);
    }
}
