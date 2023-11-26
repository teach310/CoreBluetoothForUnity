using System;
using CoreBluetooth.Foundation;

namespace CoreBluetooth
{
    /// <summary>
    /// A remote device connected to a local app, which is acting as a peripheral.
    /// https://developer.apple.com/documentation/corebluetooth/cbcentral
    /// </summary>
    public class CBCentral : IDisposable
    {
        bool _disposed = false;
        internal SafeNativeCentralHandle Handle { get; }
        NativeCentralProxy _nativeCentral = null;

        string _identifier = null;
        public string Identifier
        {
            get
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                if (_identifier == null)
                {
                    _identifier = _nativeCentral.Identifier;
                }
                return _identifier;
            }
        }

        public int MaximumUpdateValueLength
        {
            get
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                return _nativeCentral.MaximumUpdateValueLength;
            }
        }

        internal CBCentral(SafeNativeCentralHandle nativeCentral)
        {
            Handle = nativeCentral;
            _nativeCentral = new NativeCentralProxy(Handle);
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
