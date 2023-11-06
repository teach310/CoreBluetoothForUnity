using System;
using CoreBluetooth;
using NUnit.Framework;

namespace CoreBluetoothTests
{
    public class CBMutableServiceTests : CBTests
    {
#if UNITY_IOS || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        [Test]
        public void Create()
        {
            using var service = new CBMutableService(validUUID1, true);
            Assert.That(service, Is.Not.Null);
        }

        [Test]
        public void Release()
        {
            var service = new CBMutableService(validUUID1, true);
            service.Dispose();
            Assert.That(service.Handle.IsClosed, Is.True);
        }

        [Test]
        public void Characteristics_Set()
        {
            using var service = new CBMutableService(validUUID1, true);
            var characteristics = new CBMutableCharacteristic[] {
                new CBMutableCharacteristic(validUUID2, CBCharacteristicProperties.Read, null, CBAttributePermissions.Readable),
                new CBMutableCharacteristic(validUUID3, CBCharacteristicProperties.Write, null, CBAttributePermissions.Writeable)
            };

            service.Characteristics = characteristics;
            Assert.That(service.Characteristics, Is.EqualTo(characteristics));

            service.Characteristics = null;
            Assert.That(service.Characteristics, Is.Null);
            foreach (var item in characteristics)
            {
                item.Dispose();
            }
        }
#endif
    }
}
