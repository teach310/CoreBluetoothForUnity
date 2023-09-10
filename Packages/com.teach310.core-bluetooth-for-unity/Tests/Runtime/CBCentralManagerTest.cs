using CoreBluetooth;
using NUnit.Framework;

namespace CoreBluetoothTests
{
    public class CBCentralManagerDelegateMock : CBCentralManagerDelegate
    {
        public void DidUpdateState(CBCentralManager central) { }
    }

    public class CBCentralManagerTest
    {
        [Test]
        public void Create()
        {
            using var centralManager = CBCentralManager.Create();
            Assert.That(centralManager, Is.Not.Null);
        }

        [Test]
        public void Release()
        {
            CBCentralManager centralManager;
            using (centralManager = CBCentralManager.Create()) { }
            var delegateMock = new CBCentralManagerDelegateMock();
            Assert.That(() => centralManager.centralManagerDelegate = delegateMock, Throws.TypeOf<System.ObjectDisposedException>());
        }
    }
}
