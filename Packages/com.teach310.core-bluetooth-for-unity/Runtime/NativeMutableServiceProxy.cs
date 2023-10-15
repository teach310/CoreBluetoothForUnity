using System;

namespace CoreBluetooth
{
    internal class NativeMutableServiceProxy
    {
        SafeNativeMutableServiceHandle _handle;

        internal NativeMutableServiceProxy(SafeNativeMutableServiceHandle handle)
        {
            _handle = handle;
        }

        internal void SetCharacteristics(CBCharacteristic[] characteristics)
        {
            if (characteristics == null)
            {
                NativeMethods.cb4u_mutable_service_clear_characteristics(_handle);
                return;
            }

            var mutableCharacteristics = new CBMutableCharacteristic[characteristics.Length];
            for (int i = 0; i < characteristics.Length; i++)
            {
                mutableCharacteristics[i] = ConvertToCBMutableCharacteristic(characteristics[i]);
            }

            NativeMethods.cb4u_mutable_service_clear_characteristics(_handle);
            foreach (var characteristic in mutableCharacteristics)
            {
                NativeMethods.cb4u_mutable_service_add_characteristic(_handle, characteristic.Handle);
            }
        }

        CBMutableCharacteristic ConvertToCBMutableCharacteristic(CBCharacteristic characteristic)
        {
            var mutableCharacteristic = characteristic as CBMutableCharacteristic;
            if (mutableCharacteristic == null)
            {
                throw new ArgumentException("characteristics must be CBMutableCharacteristic instances");
            }
            return mutableCharacteristic;
        }
    }
}
