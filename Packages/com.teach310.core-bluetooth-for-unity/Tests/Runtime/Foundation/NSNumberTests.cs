using CoreBluetooth.Foundation;
using NUnit.Framework;

namespace CoreBluetoothTests.Foundation
{
    public class NSNumberTests
    {
#if UNITY_IOS || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        [Test]
        public void IntValue()
        {
            using var nsNumber = new NSNumber(2);
            Assert.That(nsNumber.Handle, Is.Not.Null);
            Assert.That(nsNumber.Handle.IsInvalid, Is.False);
            Assert.That(nsNumber.IntValue, Is.EqualTo(2));
        }

        [Test]
        public void BoolValue()
        {
            using var trueNumber = new NSNumber(true);
            Assert.That(trueNumber.Handle, Is.Not.Null);
            Assert.That(trueNumber.Handle.IsInvalid, Is.False);
            Assert.That(trueNumber.BoolValue, Is.True);

            using var falseNumber = new NSNumber(false);
            Assert.That(falseNumber.Handle, Is.Not.Null);
            Assert.That(falseNumber.Handle.IsInvalid, Is.False);
            Assert.That(falseNumber.BoolValue, Is.False);
        }
#endif
    }
}
