using CoreBluetooth;
using NUnit.Framework;

namespace CoreBluetoothTests
{
    public class StringExtensionsTests
    {
        [Test]
        public void ValidCBUUID_ReturnsTrue()
        {
            string validUUID1 = "6ba7b810-9dad-11d1-80b4-00c04fd430c8";
            string validUUID2 = "abcd";
            string validUUID3 = "12345678";

            Assert.That(validUUID1.IsCBUUID(), Is.True);
            Assert.That(validUUID2.IsCBUUID(), Is.True);
            Assert.That(validUUID3.IsCBUUID(), Is.True);
        }

        [Test]
        public void InvalidCBUUID_ReturnsFalse()
        {
            string invalidUUID1 = "6ba7b81-09dad-11d1-80b4-00c04fd430c8"; // ハイフンの位置が間違っている
            string invalidUUID2 = "abcg"; // 無効な文字gが含まれている
            string invalidUUID3 = "12345"; // 無効な文字数

            Assert.That(invalidUUID1.IsCBUUID(), Is.False);
            Assert.That(invalidUUID2.IsCBUUID(), Is.False);
            Assert.That(invalidUUID3.IsCBUUID(), Is.False);
        }
    }
}
