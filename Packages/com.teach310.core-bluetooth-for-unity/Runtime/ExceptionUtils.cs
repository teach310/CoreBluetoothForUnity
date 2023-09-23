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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowIfPeripheralNotFound(int result, string peripheralId)
        {
            if (result == -1)
            {
                throw new Exception($"Peripheral not found: {peripheralId}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowIfServiceNotFound(int result, string serviceUUID)
        {
            if (result == -2)
            {
                throw new Exception($"Service not found: {serviceUUID}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowIfCharacteristicNotFound(int result, string characteristicUUID)
        {
            if (result == -3)
            {
                throw new Exception($"Characteristic not found: {characteristicUUID}");
            }
        }
    }
}
