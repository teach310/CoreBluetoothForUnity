
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

        internal void StartAdvertising(StartAdvertisingOptions options = null)
        {
            if (options != null)
            {
                if (options.ServiceUUIDs != null)
                {
                    foreach (string uuidString in options.ServiceUUIDs)
                    {
                        ExceptionUtils.ThrowArgumentExceptionIfInvalidCBUUID(uuidString, nameof(options.ServiceUUIDs));
                    }
                }
            }
            else
            {
                options = new StartAdvertisingOptions();
            }

            NativeMethods.cb4u_peripheral_manager_start_advertising(
                _handle,
                options.LocalName,
                options.ServiceUUIDs,
                options.ServiceUUIDs?.Length ?? 0
            );
        }

        internal void StopAdvertising()
        {
            throw new System.NotImplementedException("TODO");
        }

        internal bool IsAdvertising
        {
            get
            {
                throw new System.NotImplementedException("TODO");
            }
        }
    }
}
