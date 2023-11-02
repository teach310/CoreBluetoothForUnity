using System;
using CoreBluetooth;
using NUnit.Framework;

namespace CoreBluetoothTests
{
    public class CBCharacteristicTests : CBTests
    {
#if UNITY_IOS || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        [Test]
        public void Value_Set_NotImplemented()
        {
            var characteristic = new CBCharacteristic(validUUID1, null);
            Assert.That(() => characteristic.Value = new byte[] { 0x01 }, Throws.TypeOf<NotImplementedException>());
        }

        [Test]
        public void Properties_Set_NotImplemented()
        {
            var characteristic = new CBCharacteristic(validUUID1, null);
            Assert.That(() => characteristic.Properties = CBCharacteristicProperties.Broadcast, Throws.TypeOf<NotImplementedException>());
        }
#endif
    }
}
