using System;

namespace CoreBluetooth
{
    internal class NativeMutableServiceProxy : INativeMutableService
    {
        SafeNativeMutableServiceHandle _handle;

        internal NativeMutableServiceProxy(SafeNativeMutableServiceHandle handle)
        {
            _handle = handle;
        }

        void INativeMutableService.SetCharacteristics(CBCharacteristic[] characteristics)
        {
            throw new NotImplementedException("TODO");
        }
    }
}
