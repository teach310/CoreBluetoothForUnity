
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    internal interface INativePeripheralManagerCallbacks
    {
        void Register(SafeNativePeripheralManagerHandle handle, INativePeripheralManagerDelegate peripheralManagerDelegate);
        void Unregister(SafeNativePeripheralManagerHandle handle);
    }

    internal class NativePeripheralManagerProxy : IDisposable
    {
        readonly SafeNativePeripheralManagerHandle _handle;
        readonly INativePeripheralManagerCallbacks _callbacks;

        internal NativePeripheralManagerProxy(SafeNativePeripheralManagerHandle handle, INativePeripheralManagerDelegate peripheralManagerDelegate)
        {
            _handle = handle;
            _callbacks = ServiceLocator.Instance.Resolve<INativePeripheralManagerCallbacks>();
            if (_callbacks != null)
            {
                _callbacks.Register(handle, peripheralManagerDelegate);
            }
        }

        internal void AddService(SafeNativeMutableServiceHandle service)
        {
            NativeMethods.cb4u_peripheral_manager_add_service(_handle, service);
        }

        internal void RemoveService(SafeNativeMutableServiceHandle service)
        {
            NativeMethods.cb4u_peripheral_manager_remove_service(_handle, service);
        }

        internal void RemoveAllServices()
        {
            NativeMethods.cb4u_peripheral_manager_remove_all_services(_handle);
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
            NativeMethods.cb4u_peripheral_manager_stop_advertising(_handle);
        }

        internal bool IsAdvertising => NativeMethods.cb4u_peripheral_manager_is_advertising(_handle);

        internal bool UpdateValue(byte[] value, SafeNativeMutableCharacteristicHandle characteristic, SafeNativeCentralHandle[] subscribedCentrals = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            IntPtr[] subscribedCentralsPtr = null;
            if (subscribedCentrals != null)
            {
                subscribedCentralsPtr = new IntPtr[subscribedCentrals.Length];
                for (int i = 0; i < subscribedCentrals.Length; i++)
                {
                    subscribedCentralsPtr[i] = subscribedCentrals[i].DangerousGetHandle();
                }
            }

            return NativeMethods.cb4u_peripheral_manager_update_value(
                _handle,
                value,
                value.Length,
                characteristic,
                subscribedCentralsPtr,
                subscribedCentrals?.Length ?? 0
            );
        }

        internal void RespondToRequest(CBATTRequest request, CBATTError result)
        {
            NativeMethods.cb4u_peripheral_manager_respond_to_request(_handle, request.Handle, (int)result);
        }

        public void Dispose()
        {
            _callbacks?.Unregister(_handle);
        }
    }
}
