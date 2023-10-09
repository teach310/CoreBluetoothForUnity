using System.Text;

namespace CoreBluetooth
{
    public class NativeCentralProxy : INativeCentral
    {
        SafeNativeCentralHandle _handle;

        internal NativeCentralProxy(SafeNativeCentralHandle handle)
        {
            _handle = handle;
        }

        public string Identifier
        {
            get
            {
                var sb = new StringBuilder(64);
                NativeMethods.cb4u_central_identifier(_handle, sb, sb.Capacity);
                return sb.ToString();
            }
        }

        public int MaximumUpdateValueLength => NativeMethods.cb4u_central_maximum_update_value_length(_handle);
    }
}
