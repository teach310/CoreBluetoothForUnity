using System;
using System.Linq;

namespace CoreBluetooth
{
    internal class CBATTRequests : IDisposable
    {
        bool _disposed = false;
        SafeNativeATTRequestsHandle _handle;
        NativeATTRequestsProxy _nativeATTRequests;

        CBATTRequest[] _requests = null;
        public CBATTRequest[] Requests
        {
            get
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                if (_requests == null)
                {
                    _requests = _nativeATTRequests.GetRequests();
                }
                return _requests.ToArray();
            }
        }

        internal CBATTRequests(SafeNativeATTRequestsHandle handle, NativeATTRequestsProxy nativeATTRequests)
        {
            _handle = handle;
            _nativeATTRequests = nativeATTRequests;
        }

        public void Dispose()
        {
            if (_disposed) return;

            if (_requests != null)
            {
                foreach (var request in _requests)
                {
                    request.Dispose();
                }
            }

            _handle?.Dispose();

            _disposed = true;
        }
    }
}
