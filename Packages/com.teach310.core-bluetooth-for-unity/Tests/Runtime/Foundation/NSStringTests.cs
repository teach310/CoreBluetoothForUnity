using CoreBluetooth.Foundation;
using NUnit.Framework;

namespace CoreBluetoothTests.Foundation
{
    public class NSStringTests
    {
        [Test]
        public void New()
        {
            using var nsString = new NSString("dummy");
            Assert.That(nsString.Handle, Is.Not.Null);
            Assert.That(nsString.Handle.IsInvalid, Is.False);
        }

        [Test]
        public void LengthOfBytesUtf8()
        {
            using (var nsString = new NSString("a"))
            {
                Assert.That(nsString.LengthOfBytesUtf8(), Is.EqualTo(1));
            }

            using (var nsString = new NSString("あ"))
            {
                Assert.That(nsString.LengthOfBytesUtf8(), Is.EqualTo(3));
            }

            using (var nsString = new NSString("𠮷"))
            {
                Assert.That(nsString.LengthOfBytesUtf8(), Is.EqualTo(4));
            }
        }

        [Test]
        public void HandleToString()
        {
            using (var nsString = new NSString("dummy"))
            {
                Assert.That(NSString.HandleToString(nsString.Handle), Is.EqualTo("dummy"));
            }

            using (var nsString = new NSString("あいうえお"))
            {
                Assert.That(NSString.HandleToString(nsString.Handle), Is.EqualTo("あいうえお"));
            }

            using (var nsString = new NSString(string.Empty))
            {
                Assert.That(NSString.HandleToString(nsString.Handle), Is.EqualTo(string.Empty));
            }
        }
    }
}
