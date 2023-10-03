using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    public class SafeNativePeripheralManagerHandle : SafeHandle
    {
        static Dictionary<IntPtr, CBPeripheralManager> s_peripheralManagerMap = new Dictionary<IntPtr, CBPeripheralManager>();

        public override bool IsInvalid => handle == IntPtr.Zero;

        SafeNativePeripheralManagerHandle(IntPtr handle) : base(handle, true) { }

        internal static SafeNativePeripheralManagerHandle Create(CBPeripheralManager peripheralManager)
        {
            var handle = NativeMethods.cb4u_peripheral_manager_new();
            var instance = new SafeNativePeripheralManagerHandle(handle);
            RegisterHandlers(instance);
            s_peripheralManagerMap.Add(handle, peripheralManager);
            return instance;
        }

        protected override bool ReleaseHandle()
        {
            s_peripheralManagerMap.Remove(handle);
            NativeMethods.cb4u_peripheral_manager_release(handle);
            return true;
        }

        static void RegisterHandlers(SafeNativePeripheralManagerHandle handle)
        {
            NativeMethods.cb4u_peripheral_manager_register_handlers(
                handle,
                DidUpdateState,
                DidAddService
            );
        }

        static CBPeripheralManager GetPeripheralManager(IntPtr peripheralPtr)
        {
            if (!s_peripheralManagerMap.TryGetValue(peripheralPtr, out var peripheralManager))
            {
                UnityEngine.Debug.LogError("CBPeripheralManager instance not found.");
            }
            return peripheralManager;
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidUpdateStateHandler))]
        internal static void DidUpdateState(IntPtr peripheralPtr, CBManagerState state)
        {
            GetPeripheralManager(peripheralPtr)?.DidUpdateState(state);
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UPeripheralManagerDidAddServiceHandler))]
        internal static void DidAddService(IntPtr peripheralPtr, string serviceUUID, int errorCode)
        {
            GetPeripheralManager(peripheralPtr)?.DidAddService(serviceUUID, CBError.CreateOrNullFromCode(errorCode));
        }
    }
}
