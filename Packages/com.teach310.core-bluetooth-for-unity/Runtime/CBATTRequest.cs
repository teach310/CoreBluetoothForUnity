using System;
using CoreBluetooth.Foundation;

namespace CoreBluetooth
{
    /// <summary>
    /// A request that uses the Attribute Protocol (ATT).
    /// https://developer.apple.com/documentation/corebluetooth/cbattrequest
    /// </summary>
    public class CBATTRequest : IDisposable
    {
        bool _disposed = false;
        internal SafeNativeATTRequestHandle Handle { get; }
        NativeATTRequestProxy _nativeATTRequest = null;

        public CBCentral Central
        {
            get
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                return _nativeATTRequest.Central;
            }
        }

        public CBCharacteristic Characteristic
        {
            get
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                return _nativeATTRequest.Characteristic;
            }
        }

        public byte[] Value
        {
            get
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                return _nativeATTRequest.Value;
            }
            set
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                _nativeATTRequest.SetValue(value);
            }
        }

        public int Offset
        {
            get
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                return _nativeATTRequest.Offset;
            }
        }

        internal CBATTRequest(SafeNativeATTRequestHandle handle, NativeATTRequestProxy nativeATTRequest)
        {
            Handle = handle;
            _nativeATTRequest = nativeATTRequest;
        }

        public override string ToString() => NSObject.ToString(this, Handle);

        public void Dispose()
        {
            if (_disposed) return;

            Handle?.Dispose();

            _disposed = true;
        }
    }
}
