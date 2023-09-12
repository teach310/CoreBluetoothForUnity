using System;
using System.Collections;
using CoreBluetooth;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CoreBluetoothTests
{
    public class CBCentralManagerDelegateMock : CBCentralManagerDelegate
    {
        public CBManagerState state { get; private set; } = CBManagerState.unknown;

        public void DidUpdateState(CBCentralManager central) => state = central.state;
    }

    public class CBCentralManagerTests
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

        [UnityTest]
        public IEnumerator DidUpdateState_CalledOnCreateAsync()
        {
            var delegateMock = new CBCentralManagerDelegateMock();
            using var centralManager = CBCentralManager.Create(delegateMock);
            Assert.That(delegateMock.state, Is.EqualTo(CBManagerState.unknown));

            yield return WaitUntilWithTimeout(() => delegateMock.state != CBManagerState.unknown, 1f);
            Assert.That(delegateMock.state, Is.Not.EqualTo(CBManagerState.unknown));
        }

        IEnumerator WaitUntilWithTimeout(Func<bool> predicate, float timeout)
        {
            var elapsedTime = 0f;
            while (!predicate() && elapsedTime < timeout)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}
