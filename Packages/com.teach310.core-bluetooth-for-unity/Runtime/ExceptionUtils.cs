using System;
using System.Runtime.CompilerServices;

namespace CoreBluetooth
{
    internal static class ExceptionUtils
    {
        // https://learn.microsoft.com/ja-jp/dotnet/api/system.objectdisposedexception.throwif?view=net-7.0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowObjectDisposedExceptionIf(bool condition, object instance)
        {
            if (condition)
            {
                throw new ObjectDisposedException(instance.GetType().FullName);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowArgumentExceptionIfInvalidCBUUID(string uuidString, string paramName = null)
        {
            if (!uuidString.IsCBUUID())
            {
                throw new ArgumentException($"Invalid UUID: {uuidString}", paramName);
            }
        }
    }
}
