using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    public class SafeNativeCentralManagerHandle : SafeHandle
    {
        static Dictionary<IntPtr, CBCentralManager> s_centralManagerMap = new Dictionary<IntPtr, CBCentralManager>();

        public override bool IsInvalid => handle == IntPtr.Zero;

        SafeNativeCentralManagerHandle(IntPtr handle) : base(handle, true) { }

        internal static SafeNativeCentralManagerHandle Create(CBCentralManager centralManager)
        {
            var handle = NativeMethods.cb4u_central_manager_new();
            var instance = new SafeNativeCentralManagerHandle(handle);
            RegisterHandlers(instance);
            s_centralManagerMap.Add(handle, centralManager);
            return instance;
        }

        protected override bool ReleaseHandle()
        {
            s_centralManagerMap.Remove(handle);
            NativeMethods.cb4u_central_manager_release(handle);
            return true;
        }

        static void RegisterHandlers(SafeNativeCentralManagerHandle handle)
        {
            NativeMethods.cb4u_central_manager_register_handlers(
                handle,
                OnDidUpdateState
            );
        }

        static CBCentralManager GetCentralManager(IntPtr centralPtr)
        {
            if (!s_centralManagerMap.TryGetValue(centralPtr, out var centralManager))
            {
                UnityEngine.Debug.LogError("CBCentralManager instance not found.");
            }
            return centralManager;
        }

        [AOT.MonoPInvokeCallback(typeof(NativeMethods.CB4UCentralManagerDidUpdateStateHandler))]
        internal static void OnDidUpdateState(IntPtr centralPtr, CBManagerState state)
        {
            GetCentralManager(centralPtr)?.OnDidUpdateState(state);
        }
    }
}
