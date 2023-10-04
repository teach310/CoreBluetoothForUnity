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
        public CBService AddedService { get; private set; }
        public int DidStartAdvertisingCount { get; private set; } = 0;
        public CBError Error { get; private set; }

        public void DidUpdateState(CBPeripheralManager peripheral) => State = peripheral.State;
        public void DidAddService(CBPeripheralManager peripheral, CBService service, CBError error) => AddedService = service;
        public void DidStartAdvertising(CBPeripheralManager peripheral, CBError error)
        {
            DidStartAdvertisingCount++;
            Error = error;
        }
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

        [UnityTest]
        public IEnumerator AddService()
        {
            var delegateMock = new CBPeripheralManagerDelegateMock();
            using var peripheralManager = new CBPeripheralManager(delegateMock);

            yield return WaitUntilWithTimeout(() => delegateMock.State != CBManagerState.Unknown, 1f);
            if (delegateMock.State != CBManagerState.PoweredOn) yield break;

            using var service = new CBMutableService(validUUID1, true);
            using var characteristic1 = new CBMutableCharacteristic(validUUID2, CBCharacteristicProperties.Read, null, CBAttributePermissions.Readable);
            using var characteristic2 = new CBMutableCharacteristic(validUUID3, CBCharacteristicProperties.Write, null, CBAttributePermissions.Writeable);
            var characteristics = new CBMutableCharacteristic[] { characteristic1, characteristic2 };
            service.Characteristics = characteristics;
            peripheralManager.AddService(service);

            yield return WaitUntilWithTimeout(() => delegateMock.AddedService != null, 2f);
            Assert.That(delegateMock.AddedService, Is.EqualTo(service));
        }

        [UnityTest]
        public IEnumerator Advertising()
        {
            var delegateMock = new CBPeripheralManagerDelegateMock();
            using var peripheralManager = new CBPeripheralManager(delegateMock);

            yield return WaitUntilWithTimeout(() => delegateMock.State != CBManagerState.Unknown, 1f);
            if (delegateMock.State != CBManagerState.PoweredOn) yield break;

            using var service = new CBMutableService(validUUID1, true);
            using var characteristic1 = new CBMutableCharacteristic(validUUID2, CBCharacteristicProperties.Read, null, CBAttributePermissions.Readable);
            using var characteristic2 = new CBMutableCharacteristic(validUUID3, CBCharacteristicProperties.Write, null, CBAttributePermissions.Writeable);
            var characteristics = new CBMutableCharacteristic[] { characteristic1, characteristic2 };
            service.Characteristics = characteristics;
            peripheralManager.AddService(service);

            yield return WaitUntilWithTimeout(() => delegateMock.AddedService != null, 1f);
            var options = new StartAdvertisingOptions()
            {
                LocalName = "Test",
                ServiceUUIDs = new string[] { validUUID1 }
            };
            peripheralManager.StartAdvertising(options);
            Assert.That(peripheralManager.IsAdvertising, Is.False);
            yield return WaitUntilWithTimeout(() => delegateMock.DidStartAdvertisingCount > 0, 1f);
            Assert.That(peripheralManager.IsAdvertising, Is.True);
            Assert.That(delegateMock.DidStartAdvertisingCount, Is.EqualTo(1));
            Assert.That(delegateMock.Error, Is.Null);
            peripheralManager.StopAdvertising();
            Assert.That(peripheralManager.IsAdvertising, Is.True);
            yield return new WaitForSeconds(0.1f);
            Assert.That(peripheralManager.IsAdvertising, Is.False);
        }

        [UnityTest]
        public IEnumerator AlreadyAdvertising()
        {
            var delegateMock = new CBPeripheralManagerDelegateMock();
            using var peripheralManager = new CBPeripheralManager(delegateMock);

            yield return WaitUntilWithTimeout(() => delegateMock.State != CBManagerState.Unknown, 1f);
            if (delegateMock.State != CBManagerState.PoweredOn) yield break;

            using var service = new CBMutableService(validUUID1, true);
            using var characteristic1 = new CBMutableCharacteristic(validUUID2, CBCharacteristicProperties.Read, null, CBAttributePermissions.Readable);
            using var characteristic2 = new CBMutableCharacteristic(validUUID3, CBCharacteristicProperties.Write, null, CBAttributePermissions.Writeable);
            var characteristics = new CBMutableCharacteristic[] { characteristic1, characteristic2 };
            service.Characteristics = characteristics;
            peripheralManager.AddService(service);

            yield return WaitUntilWithTimeout(() => delegateMock.AddedService != null, 1f);
            var options = new StartAdvertisingOptions()
            {
                LocalName = "Test",
                ServiceUUIDs = new string[] { validUUID1 }
            };
            peripheralManager.StartAdvertising(options);
            yield return WaitUntilWithTimeout(() => delegateMock.DidStartAdvertisingCount > 0, 1f);
            Assert.That(delegateMock.Error, Is.Null);
            peripheralManager.StartAdvertising(options);
            yield return WaitUntilWithTimeout(() => delegateMock.DidStartAdvertisingCount > 1, 1f);
            Assert.That(delegateMock.DidStartAdvertisingCount, Is.EqualTo(2));
            Assert.That(delegateMock.Error.ErrorCode, Is.EqualTo(CBError.Code.AlreadyAdvertising));
        }
    }
}
