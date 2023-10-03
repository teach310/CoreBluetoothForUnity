using System.Collections.Generic;
using UnityEngine;
using CoreBluetooth;
using System;

namespace CoreBluetoothSample
{
    public class Sample_Peripheral : MonoBehaviour, ICBPeripheralManagerDelegate
    {
        CBPeripheralManager peripheralManager;

        string serviceUUID = "068C47B7-FC04-4D47-975A-7952BE1A576F";
        string characteristicUUID = "E3737B3F-A08D-405B-B32D-35A8F6C64C5D";

        List<IDisposable> disposables = new List<IDisposable>();

        void Start()
        {
            peripheralManager = new CBPeripheralManager(this);
            disposables.Add(peripheralManager);
        }

        public void DidUpdateState(CBPeripheralManager peripheral)
        {
            Debug.Log($"[DidUpdateState] {peripheral.State}");
            if (peripheral.State == CBManagerState.PoweredOn)
            {
                var service = new CBMutableService(serviceUUID, true);
                var characteristic = new CBMutableCharacteristic(
                    characteristicUUID,
                    CBCharacteristicProperties.Read | CBCharacteristicProperties.Write | CBCharacteristicProperties.Notify,
                    null,
                    CBAttributePermissions.Readable | CBAttributePermissions.Writeable
                );
                service.Characteristics = new CBCharacteristic[] { characteristic };
                peripheral.AddService(service);
                disposables.Add(service);
                disposables.Add(characteristic);
            }
        }

        public void DidAddService(CBPeripheralManager peripheral, CBService service, CBError error)
        {
            Debug.Log($"[DidAddService] peripheral: {peripheral}  service: {service}  error: {error}");
        }

        void OnDestroy()
        {
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }
        }
    }
}
