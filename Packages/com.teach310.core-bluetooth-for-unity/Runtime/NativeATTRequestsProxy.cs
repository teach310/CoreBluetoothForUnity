namespace CoreBluetooth
{
    internal class NativeATTRequestsProxy : INativeATTRequests
    {
        SafeNativeATTRequestsHandle _handle;
        IPeripheralManagerData _peripheralManagerData;

        internal NativeATTRequestsProxy(SafeNativeATTRequestsHandle handle, IPeripheralManagerData peripheralManagerData)
        {
            _handle = handle;
            _peripheralManagerData = peripheralManagerData;
        }

        CBATTRequest[] INativeATTRequests.GetRequests()
        {
            int count = NativeMethods.cb4u_att_requests_count(_handle);
            CBATTRequest[] requests = new CBATTRequest[count];
            for (int i = 0; i < count; i++)
            {
                var requestPtr = NativeMethods.cb4u_att_requests_request(_handle, i);
                var requestHandle = new SafeNativeATTRequestHandle(requestPtr);
                requests[i] = new CBATTRequest(requestHandle, new NativeATTRequestProxy(requestHandle, _peripheralManagerData));
            }
            return requests;
        }
    }
}
