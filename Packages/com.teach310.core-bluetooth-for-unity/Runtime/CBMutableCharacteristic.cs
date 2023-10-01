using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CoreBluetooth
{
    internal interface INativeMutableCharacteristic : INativeCharacteristic
    {
        void SetValue(byte[] value);
        void SetProperties(CBCharacteristicProperties properties);
        void SetPermissions(CBAttributePermissions permissions);
    }

    /// <summary>
    /// A characteristic of a local peripheral’s service.
    /// https://developer.apple.com/documentation/corebluetooth/cbmutablecharacteristic
    /// </summary>
    public class CBMutableCharacteristic : CBCharacteristic, IDisposable
    {
        bool _disposed = false;
        internal SafeNativeMutableCharacteristicHandle Handle { get; }

        public override byte[] Value
        {
            get => base.Value;
            set
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                _nativeMutableCharacteristic.SetValue(value);
                UpdateValue(value);
            }
        }

        public override CBCharacteristicProperties Properties
        {
            get => base.Properties;
            set
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                _nativeMutableCharacteristic.SetProperties(value);
            }
        }

        CBAttributePermissions _permissions;
        public CBAttributePermissions Permissions
        {
            get => _permissions;
            set
            {
                ExceptionUtils.ThrowObjectDisposedExceptionIf(_disposed, this);
                _nativeMutableCharacteristic.SetPermissions(value);
                _permissions = value;
            }
        }

        INativeMutableCharacteristic _nativeMutableCharacteristic = null;

        public CBMutableCharacteristic(
            string uuid,
            CBCharacteristicProperties properties,
            byte[] value,
            CBAttributePermissions permissions
        ) : base(uuid, null, null)
        {
            Handle = SafeNativeMutableCharacteristicHandle.Create(uuid, properties, value, permissions);
            _nativeMutableCharacteristic = new NativeMutableCharacteristicProxy(Handle);
        }

        public void Dispose()
        {
            if (_disposed) return;

            Handle?.Dispose();

            _disposed = true;
        }
    }
}
