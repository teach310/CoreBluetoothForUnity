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
            IntPtr[] mutableCharacteristics = null;
            if (characteristics != null)
            {
                mutableCharacteristics = new IntPtr[characteristics.Length];
                for (int i = 0; i < characteristics.Length; i++)
                {
                    mutableCharacteristics[i] = ConvertToCBMutableCharacteristic(characteristics[i]).Handle.DangerousGetHandle();
                }
            }
            NativeMethods.cb4u_mutable_service_set_characteristics(_handle, mutableCharacteristics, mutableCharacteristics?.Length ?? 0);
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
