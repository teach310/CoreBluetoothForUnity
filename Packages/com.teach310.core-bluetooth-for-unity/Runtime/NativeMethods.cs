using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    internal static class NativeMethods
    {
#if UNITY_IOS && !UNITY_EDITOR
        const string DLL_NAME = "__Internal";
#else
        const string DLL_NAME = "CB4UBundle";
#endif
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cb4u_hello();
    }
}
