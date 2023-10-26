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
        public CBManagerState State { get; private set; } = CBManagerState.Unknown;

        public void DidUpdateState(CBCentralManager central) => State = central.State;
    }

    public class CBCentralManagerTests : CBTests
    {
        [Test]
        public void Create()
        {
            using var centralManager = new CBCentralManager();
            Assert.That(centralManager, Is.Not.Null);
        }

        [Test]
        public void CreateWithOptions()
        {
            var options = new CBCentralInitOptions() { ShowPowerAlert = true };
            using var centralManager = new CBCentralManager(null, options);
            Assert.That(centralManager, Is.Not.Null);
        }

        [Test]
        public void Release()
        {
            var centralManager = new CBCentralManager();
            centralManager.Dispose();
            var delegateMock = new CBCentralManagerDelegateMock();
            Assert.That(() => centralManager.Delegate = delegateMock, Throws.TypeOf<System.ObjectDisposedException>());
        }

        [UnityTest]
        public IEnumerator DidUpdateState_CalledAfterNewAsync()
        {
            var delegateMock = new CBCentralManagerDelegateMock();
            using var centralManager = new CBCentralManager(delegateMock);
            Assert.That(delegateMock.State, Is.EqualTo(CBManagerState.Unknown));

            yield return WaitUntilWithTimeout(() => delegateMock.State != CBManagerState.Unknown, 1f);
            Assert.That(delegateMock.State, Is.Not.EqualTo(CBManagerState.Unknown));
        }

        [UnityTest]
        public IEnumerator ScanForPeripherals_InvalidServiceUUID_Throw()
        {
            using var centralManager = new CBCentralManager();
            yield return WaitUntilWithTimeout(() => centralManager.State != CBManagerState.Unknown, 1f);
            if (centralManager.State != CBManagerState.PoweredOn) yield break;

            Assert.That(() => centralManager.ScanForPeripherals(new string[] { "invalid" }), Throws.TypeOf<ArgumentException>());
        }

        [UnityTest]
        public IEnumerator ScanStartStop()
        {
            using var centralManager = new CBCentralManager();
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
