using System;

namespace CoreBluetooth
{
    /// <summary>
    /// An error that Core Bluetooth returns during Bluetooth transactions.
    /// https://developer.apple.com/documentation/corebluetooth/cberror
    /// </summary>
    public class CBError : Exception
    {
        public enum Code
        {
            unknown = 0,
            invalidParameters = 1,
            invalidHandle = 2,
            notConnected = 3,
            outOfSpace = 4,
            operationCancelled = 5,
            connectionTimeout = 6,
            peripheralDisconnected = 7,
            uuidNotAllowed = 8,
            alreadyAdvertising = 9,
            connectionFailed = 10,
            connectionLimitReached = 11,
            operationNotSupported = 13
        }

        public CBError.Code errorCode { get; }

        public CBError(Code errorCode)
        {
            this.errorCode = errorCode;
        }

        internal static CBError CreateOrNullFromCode(int code)
        {
            if (code < 0) return null;

            if (Enum.IsDefined(typeof(Code), code))
            {
                return new CBError((Code)code);
            }
            else
            {
                return new CBError(Code.unknown);
            }
        }

        public override string ToString()
        {
            return $"CBError: {errorCode}";
        }
    }
}
