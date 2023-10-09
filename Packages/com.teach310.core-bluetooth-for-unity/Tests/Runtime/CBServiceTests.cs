using System;
using CoreBluetooth;
using NUnit.Framework;

namespace CoreBluetoothTests
{
    public class CBServiceTests : CBTests
    {
        [Test]
        public void Characteristics_Set_NotImplemented()
        {
            var service = new CBService(validUUID1, null);
            Assert.That(() => service.Characteristics = new CBCharacteristic[] { }, Throws.TypeOf<NotImplementedException>());
        }

        [Test]
        public void UpdateCharacteristics()
        {
            var service = new CBService(validUUID1, null);
            var characteristics = new CBCharacteristic[] { new CBCharacteristic(validUUID2, null), new CBCharacteristic(validUUID3, null) };
            service.UpdateCharacteristics(characteristics);
            Assert.That(service.Characteristics, Is.EqualTo(characteristics));
            foreach (var characteristic in characteristics)
            {
                Assert.That(characteristic.Service, Is.EqualTo(service));
            }

            var emptyCharacteristics = new CBCharacteristic[] { };
            service.UpdateCharacteristics(emptyCharacteristics);
            Assert.That(service.Characteristics, Is.EqualTo(emptyCharacteristics));
            foreach (var characteristic in characteristics)
            {
                Assert.That(characteristic.Service, Is.Null);
            }

            service.UpdateCharacteristics(null);
            Assert.That(service.Characteristics, Is.Null);
        }
    }
}
