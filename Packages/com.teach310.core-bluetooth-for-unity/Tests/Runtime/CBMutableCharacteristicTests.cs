using System;
using CoreBluetooth;
using NUnit.Framework;

namespace CoreBluetoothTests
{
    public class CBMutableCharacteristicTests : CBTests
    {
#if UNITY_IOS || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        [Test]
        public void Create()
        {
            var data = new byte[] { 0x01 };
            using var characteristic = new CBMutableCharacteristic(validUUID1, CBCharacteristicProperties.Broadcast, data, CBAttributePermissions.Readable);
            Assert.That(characteristic, Is.Not.Null);
            Assert.That(characteristic.Value, Is.EqualTo(data));
        }

        [Test]
        public void Release()
        {
            var characteristic = new CBMutableCharacteristic(validUUID1, CBCharacteristicProperties.Broadcast, null, CBAttributePermissions.Readable);
            characteristic.Dispose();
            Assert.That(() => characteristic.Value = new byte[] { 0x01 }, Throws.TypeOf<ObjectDisposedException>());
        }

        [Test]
        public void Value()
        {
            using var characteristic = new CBMutableCharacteristic(validUUID1, CBCharacteristicProperties.Broadcast, null, CBAttributePermissions.Readable);
            Assert.That(characteristic.Value, Is.Null);
            var data = new byte[] { 0x01, 0x02 };
            characteristic.Value = data;
            Assert.That(characteristic.Value, Is.EqualTo(data));
            data = new byte[0];
            characteristic.Value = data;
            Assert.That(characteristic.Value, Is.EqualTo(data));
            characteristic.Value = null;
            Assert.That(characteristic.Value, Is.Null);
        }

        [Test]
        public void Properties()
        {
            using var characteristic = new CBMutableCharacteristic(validUUID1, CBCharacteristicProperties.Broadcast, null, CBAttributePermissions.Readable);
            Assert.That(characteristic.Properties, Is.EqualTo(CBCharacteristicProperties.Broadcast));
            characteristic.Properties = CBCharacteristicProperties.Read | CBCharacteristicProperties.Write;
            Assert.That(characteristic.Properties, Is.EqualTo(CBCharacteristicProperties.Read | CBCharacteristicProperties.Write));
        }

        [Test]
        public void Permissions()
        {
            using var characteristic = new CBMutableCharacteristic(validUUID1, CBCharacteristicProperties.Broadcast, null, CBAttributePermissions.Readable);
            Assert.That(characteristic.Permissions, Is.EqualTo(CBAttributePermissions.Readable));
            characteristic.Permissions = CBAttributePermissions.Readable | CBAttributePermissions.Writeable;
            Assert.That(characteristic.Permissions, Is.EqualTo(CBAttributePermissions.Readable | CBAttributePermissions.Writeable));
        }

        [Test]
        public void ToString_Output()
        {
            using var characteristic = new CBMutableCharacteristic(validUUID1, CBCharacteristicProperties.Broadcast, null, CBAttributePermissions.Readable);
            Assert.That(characteristic.ToString(), Is.EqualTo($"CBMutableCharacteristic: UUID = {validUUID1}, Value = (null), Properties = 0x1, Permissions = 0x1, Descriptors = (null), SubscribedCentrals = (\n)"));
        }
#endif
    }
}
