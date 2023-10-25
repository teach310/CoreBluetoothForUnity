using CoreBluetooth.Foundation;
using NUnit.Framework;

namespace CoreBluetoothTests.Foundation
{
    public class NSMutableDictionaryTests
    {
        [Test]
        public void New()
        {
            using var nsDictionary = new NSMutableDictionary();
            Assert.That(nsDictionary.Handle, Is.Not.Null);
            Assert.That(nsDictionary.Handle.IsInvalid, Is.False);
        }

        [Test]
        public void SetAndGetStringValue()
        {
            using var nsDictionary = new NSMutableDictionary();
            using var key = new NSString("hoge");
            using var value = new NSString("fuga");
            nsDictionary.SetValue(key.Handle, value.Handle);

            using var key2 = new NSString("hoge");
            using var nSString = new NSString(nsDictionary.GetValue(key2.Handle));
            Assert.That(nSString.ToString(), Is.EqualTo("fuga"));
        }

        [Test]
        public void ReturnZeroPointerIfNotFound()
        {
            using var nsDictionary = new NSMutableDictionary();
            using var notFoundKey = new NSString("dummy");
            var gotPtr = nsDictionary.GetValue(notFoundKey.Handle);
            Assert.That(gotPtr, Is.EqualTo(System.IntPtr.Zero));
        }

        [Test]
        public void TryGetValue()
        {
            using var nsDictionary = new NSMutableDictionary();
            using var key = new NSString("hoge");
            using var value = new NSString("fuga");
            nsDictionary.SetValue(key.Handle, value.Handle);

            using var key2 = new NSString("hoge");
            Assert.That(nsDictionary.TryGetValue(key2.Handle, out var gotPtr), Is.True);
            using var nSString = new NSString(gotPtr);
            Assert.That(nSString.ToString(), Is.EqualTo("fuga"));

            using var notFoundKey = new NSString("dummy");
            Assert.That(nsDictionary.TryGetValue(notFoundKey.Handle, out gotPtr), Is.False);
        }
    }
}
