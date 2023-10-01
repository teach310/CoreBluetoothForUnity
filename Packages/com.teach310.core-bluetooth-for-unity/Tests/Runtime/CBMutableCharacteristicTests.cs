using System;
using CoreBluetooth;
using NUnit.Framework;

namespace CoreBluetoothTests
{
    public class CBMutableCharacteristicTests : CBTests
    {
        [Test]
        public void Create()
        {
            using var characteristic = new CBMutableCharacteristic(validUUID1, CBCharacteristicProperties.Broadcast, null, CBAttributePermissions.Readable);
            Assert.That(characteristic, Is.Not.Null);
        }

        [Test]
        public void Release()
        {
            var characteristic = new CBMutableCharacteristic(validUUID1, CBCharacteristicProperties.Broadcast, null, CBAttributePermissions.Readable);
            characteristic.Dispose();
            Assert.That(() => characteristic.Value = new byte[] { 0x01 }, Throws.TypeOf<ObjectDisposedException>());
        }
    }
}
