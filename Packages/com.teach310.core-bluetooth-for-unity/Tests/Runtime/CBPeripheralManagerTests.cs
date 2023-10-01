using System;
using System.Collections;
using CoreBluetooth;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CoreBluetoothTests
{
    public class CBPeripheralManagerDelegateMock : ICBPeripheralManagerDelegate
    {
        public CBManagerState State { get; private set; } = CBManagerState.Unknown;

        public void DidUpdateState(CBPeripheralManager peripheral) => State = peripheral.State;
    }

    public class CBPeripheralManagerTests : CBTests
    {
        [Test]
        public void Create()
        {
            using var peripheralManager = new CBPeripheralManager();
            Assert.That(peripheralManager, Is.Not.Null);
        }

        [Test]
        public void Release()
        {
            var peripheralManager = new CBPeripheralManager();
            peripheralManager.Dispose();
            var delegateMock = new CBPeripheralManagerDelegateMock();
            Assert.That(() => peripheralManager.Delegate = delegateMock, Throws.TypeOf<System.ObjectDisposedException>());
        }

        [UnityTest]
        public IEnumerator DidUpdateState_CalledAfterNewAsync()
        {
            var delegateMock = new CBPeripheralManagerDelegateMock();
            using var peripheralManager = new CBPeripheralManager(delegateMock);
            Assert.That(delegateMock.State, Is.EqualTo(CBManagerState.Unknown));

            yield return WaitUntilWithTimeout(() => delegateMock.State != CBManagerState.Unknown, 1f);
            Assert.That(delegateMock.State, Is.Not.EqualTo(CBManagerState.Unknown));
        }
    }
}
