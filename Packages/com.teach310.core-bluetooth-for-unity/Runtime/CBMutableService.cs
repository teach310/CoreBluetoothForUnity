using System;

namespace CoreBluetooth
{
    internal interface INativeMutableService
    {
        void SetCharacteristics(CBCharacteristic[] characteristics);
    }

    /// <summary>
    /// A service with writeable property values.
    /// https://developer.apple.com/documentation/corebluetooth/cbmutableservice
    /// </summary>
    public class CBMutableService : CBService, IDisposable
    {
        bool _disposed = false;
        internal SafeNativeMutableServiceHandle Handle { get; }
        readonly INativeMutableService _nativeMutableService;

        public override CBCharacteristic[] Characteristics
        {
            get
            {
                return base.Characteristics;
            }
            set
            {
                _nativeMutableService.SetCharacteristics(value);
                UpdateCharacteristics(value);
            }
        }

        public CBMutableService(string uuid, bool isPrimary) : base(uuid)
        {
            Handle = SafeNativeMutableServiceHandle.Create(uuid, isPrimary);
            _nativeMutableService = new NativeMutableServiceProxy(Handle);
        }

        public override string ToString()
        {
            return $"CBMutableService: UUID = {UUID}";
        }

        public void Dispose()
        {
            if (_disposed) return;

            Handle?.Dispose();

            _disposed = true;
        }
    }
}
