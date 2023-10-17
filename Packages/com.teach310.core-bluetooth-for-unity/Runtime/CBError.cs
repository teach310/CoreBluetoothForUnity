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
            Unknown = 0,
            InvalidParameters = 1,
            InvalidHandle = 2,
            NotConnected = 3,
            OutOfSpace = 4,
            OperationCancelled = 5,
            ConnectionTimeout = 6,
            PeripheralDisconnected = 7,
            UUIDNotAllowed = 8,
            AlreadyAdvertising = 9,
            ConnectionFailed = 10,
            ConnectionLimitReached = 11,
            UnkownDevice = 12,
            OperationNotSupported = 13
        }

        public CBError.Code ErrorCode { get; }

        public CBError(Code errorCode)
        {
            this.ErrorCode = errorCode;
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
                return new CBError(Code.Unknown);
            }
        }

        public override string ToString()
        {
            return $"CBError: {ErrorCode}";
        }
    }
}
