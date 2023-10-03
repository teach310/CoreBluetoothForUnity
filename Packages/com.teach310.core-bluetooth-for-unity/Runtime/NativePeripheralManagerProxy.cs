
namespace CoreBluetooth
{
    internal class NativePeripheralManagerProxy
    {
        readonly SafeNativePeripheralManagerHandle _handle;

        internal NativePeripheralManagerProxy(SafeNativePeripheralManagerHandle handle)
        {
            _handle = handle;
        }

        internal void AddService(CBMutableService service)
        {
            NativeMethods.cb4u_peripheral_manager_add_service(_handle, service.Handle);
        }
    }
}
