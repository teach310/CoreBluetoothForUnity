using CoreBluetooth.Foundation;
using NUnit.Framework;

namespace CoreBluetoothTests.Foundation
{
    public class NSArrayTests
    {
#if UNITY_IOS || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        [Test]
        public void FromStrings()
        {
            using var nsArray = NSArray.FromStrings(new[] { "hoge", "fuga" });
            Assert.That(nsArray.Handle, Is.Not.Null);
            Assert.That(nsArray.Handle.IsInvalid, Is.False);
        }

        [Test]
        public void StringsFromHandle()
        {
            using var nsArray = NSArray.FromStrings(new[] { "hoge", "fuga" });
            var strings = NSArray.StringsFromHandle(nsArray.Handle);
            Assert.That(strings, Is.EqualTo(new[] { "hoge", "fuga" }));
        }
#endif
    }
}
