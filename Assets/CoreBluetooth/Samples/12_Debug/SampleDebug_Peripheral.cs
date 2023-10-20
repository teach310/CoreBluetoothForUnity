using System;
using System.Collections.Generic;
using System.Text;
using CoreBluetooth;
using UnityEngine;

namespace CoreBluetoothSample
{
    public class SampleDebug_Peripheral : MonoBehaviour, ICBPeripheralManagerDelegate
    {
        string _serviceUUID = "068C47B7-FC04-4D47-975A-7952BE1A576F";
        string _characteristicUUID = "E3737B3F-A08D-405B-B32D-35A8F6C64C5D";

        CBPeripheralManager _peripheralManager;
        CBCentral _central = null;
        CBMutableCharacteristic _characteristic = null;

        List<IDisposable> _disposables = new List<IDisposable>();
        byte[] _value = null;

        void Start()
        {
            _peripheralManager = new CBPeripheralManager(this);
            _disposables.Add(_peripheralManager);
        }

        public void DidUpdateState(CBPeripheralManager peripheral)
        {
            Debug.Log($"[DidUpdateState] {peripheral.State}");
            if (peripheral.State == CBManagerState.PoweredOn)
            {
                var service = new CBMutableService(_serviceUUID, true);
                _characteristic = new CBMutableCharacteristic(
                    _characteristicUUID,
                    CBCharacteristicProperties.Read | CBCharacteristicProperties.WriteWithoutResponse | CBCharacteristicProperties.Notify,
                    null,
                    CBAttributePermissions.Readable | CBAttributePermissions.Writeable
                );
                service.Characteristics = new CBCharacteristic[] { _characteristic };
                peripheral.AddService(service);
                _disposables.Add(service);
                _disposables.Add(_characteristic);
            }
        }

        public void DidAddService(CBPeripheralManager peripheral, CBService service, CBError error)
        {
            Debug.Log($"[DidAddService] peripheral: {peripheral}  service: {service}  error: {error}");
            if (error == null)
            {
                var options = new StartAdvertisingOptions()
                {
                    LocalName = "Unity",
                    ServiceUUIDs = new string[] { _serviceUUID }
                };
                peripheral.StartAdvertising(options);
            }
        }

        public void DidStartAdvertising(CBPeripheralManager peripheral, CBError error)
        {
            Debug.Log($"[DidStartAdvertising] peripheral: {peripheral}  error: {error}");
        }

        public void DidSubscribeToCharacteristic(CBPeripheralManager peripheral, CBCentral central, CBCharacteristic characteristic)
        {
            Debug.Log($"[DidSubscribeToCharacteristic] peripheral: {peripheral}  central: {central}  characteristic: {characteristic}");
            _central = central;
        }

        public void DidUnsubscribeFromCharacteristic(CBPeripheralManager peripheral, CBCentral central, CBCharacteristic characteristic)
        {
            Debug.Log($"[DidUnsubscribeFromCharacteristic] peripheral: {peripheral}  central: {central}  characteristic: {characteristic}");
            _central = null;
        }

        public void IsReadyToUpdateSubscribers(CBPeripheralManager peripheral)
        {
            Debug.Log($"[IsReadyToUpdateSubscribers] {peripheral}");
        }

        public void DidReceiveReadRequest(CBPeripheralManager peripheral, CBATTRequest request)
        {
            if (request.Characteristic.UUID != _characteristicUUID)
            {
                peripheral.RespondToRequest(request, CBATTError.AttributeNotFound);
                return;
            }

            int valueLength = _value?.Length ?? 0;
            if (request.Offset > valueLength)
            {
                peripheral.RespondToRequest(request, CBATTError.InvalidOffset);
                return;
            }

            request.Value = _value ?? new byte[0];
            peripheral.RespondToRequest(request, CBATTError.Success);
            Debug.Log($"[DidReceiveReadRequest] {System.Text.Encoding.UTF8.GetString(request.Value)}");
        }

        public void DidReceiveWriteRequests(CBPeripheralManager peripheral, CBATTRequest[] requests)
        {
            var firstRequest = requests[0];
            foreach (var request in requests)
            {
                if (request.Characteristic.UUID != _characteristicUUID)
                {
                    peripheral.RespondToRequest(firstRequest, CBATTError.AttributeNotFound);
                    return;
                }
                int valueLength = _value?.Length ?? 0;
                if (request.Offset > valueLength)
                {
                    peripheral.RespondToRequest(firstRequest, CBATTError.InvalidOffset);
                    return;
                }
            }

            _value = firstRequest.Value;
            peripheral.RespondToRequest(firstRequest, CBATTError.Success);
            Debug.Log($"[DidReceiveWriteRequests] {System.Text.Encoding.UTF8.GetString(firstRequest.Value)}");
        }

        public void OnClickNotify()
        {
            if (_central == null)
            {
                return;
            }

            var value = UnityEngine.Random.Range(100, 1000).ToString();
            var data = Encoding.UTF8.GetBytes(value);
            _value = data;

            _peripheralManager.UpdateValue(_value, _characteristic, new CBCentral[] { _central });
        }

        public void OnClickRemoveAllServices()
        {
            _peripheralManager.RemoveAllServices();
        }

        void OnDestroy()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}
