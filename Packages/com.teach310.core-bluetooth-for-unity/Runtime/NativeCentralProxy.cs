using System.Text;

namespace CoreBluetooth
{
    internal class NativeCentralProxy
    {
        SafeNativeCentralHandle _handle;

        internal NativeCentralProxy(SafeNativeCentralHandle handle)
        {
            _handle = handle;
        }

        internal string Identifier
        {
            get
            {
                var sb = new StringBuilder(64);
                NativeMethods.cb4u_central_identifier(_handle, sb, sb.Capacity);
                return sb.ToString();
            }
        }

        internal int MaximumUpdateValueLength => NativeMethods.cb4u_central_maximum_update_value_length(_handle);
    }
}
