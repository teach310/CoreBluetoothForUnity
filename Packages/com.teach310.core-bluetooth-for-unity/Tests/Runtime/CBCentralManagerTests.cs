using System;
using System.Collections;
using CoreBluetooth;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CoreBluetoothTests
{
    public class CBCentralManagerDelegateMock : ICBCentralManagerDelegate
    {
        public CBManagerState state { get; private set; } = CBManagerState.Unknown;

        public void DidUpdateState(CBCentralManager central) => state = central.State;
    }

    public class CBCentralManagerTests : CBTests
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
            Assert.That(() => centralManager.Delegate = delegateMock, Throws.TypeOf<System.ObjectDisposedException>());
        }

        [UnityTest]
        public IEnumerator DidUpdateState_CalledOnCreateAsync()
        {
            var delegateMock = new CBCentralManagerDelegateMock();
            using var centralManager = CBCentralManager.Create(delegateMock);
            Assert.That(delegateMock.state, Is.EqualTo(CBManagerState.Unknown));

            yield return WaitUntilWithTimeout(() => delegateMock.state != CBManagerState.Unknown, 1f);
            Assert.That(delegateMock.state, Is.Not.EqualTo(CBManagerState.Unknown));
        }

        [UnityTest]
        public IEnumerator ScanForPeripherals_InvalidServiceUUID_Throw()
        {
            using var centralManager = CBCentralManager.Create();
            yield return WaitUntilWithTimeout(() => centralManager.State != CBManagerState.Unknown, 1f);
            if (centralManager.State != CBManagerState.PoweredOn) yield break;

            Assert.That(() => centralManager.ScanForPeripherals(new string[] { "invalid" }), Throws.TypeOf<ArgumentException>());
        }

        [UnityTest]
        public IEnumerator ScanStartStop()
        {
            using var centralManager = CBCentralManager.Create();
            yield return WaitUntilWithTimeout(() => centralManager.State != CBManagerState.Unknown, 1f);
            if (centralManager.State != CBManagerState.PoweredOn) yield break;

            Assert.That(centralManager.IsScanning, Is.False);

            centralManager.ScanForPeripherals(new string[] { validUUID1 });
            Assert.That(centralManager.IsScanning, Is.True);

            centralManager.StopScan();
            Assert.That(centralManager.IsScanning, Is.False);
        }
    }
}
