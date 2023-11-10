using System;
using System.Collections.Generic;
using CoreBluetooth;
using UnityEngine;
using UnityEngine.UI;

namespace CoreBluetoothSample
{
    public class SampleLightControl_Peripheral : MonoBehaviour, ICBPeripheralManagerDelegate
    {
        [SerializeField] Light _light;
        [SerializeField] SampleLightControl_Header _header;
        [SerializeField] Toggle _advertiseToggle;

        CBPeripheralManager _peripheralManager;
        CBMutableService _lightControlService;

        List<IDisposable> _disposables = new List<IDisposable>();

        void Start()
        {
            var initOptions = new CBPeripheralManagerInitOptions() { ShowPowerAlert = true };
            _peripheralManager = new CBPeripheralManager(this, initOptions);
            _disposables.Add(_peripheralManager);

            _advertiseToggle.onValueChanged.AddListener(OnAdvertiseToggleValueChanged);
        }

        void StartAdvertising()
        {
            var options = new StartAdvertisingOptions()
            {
                ServiceUUIDs = new string[] { SampleLightControl_Data.ServiceUUID }
            };
            _peripheralManager.StartAdvertising(options);
        }

        void ICBPeripheralManagerDelegate.DidUpdateState(CBPeripheralManager peripheral)
        {
            if (peripheral.State == CBManagerState.PoweredOn)
            {
                if (_lightControlService == null)
                {
                    _lightControlService = new CBMutableService(SampleLightControl_Data.ServiceUUID, true);
                    var characteristic = new CBMutableCharacteristic(
                        SampleLightControl_Data.LightControlCharacteristicUUID,
                        CBCharacteristicProperties.Write,
                        null,
                        CBAttributePermissions.Writeable
                    );
                    _lightControlService.Characteristics = new CBCharacteristic[] { characteristic };
                    _peripheralManager.AddService(_lightControlService);
                    _disposables.Add(_lightControlService);
                    _disposables.Add(characteristic);
                }
            }
        }

        void ICBPeripheralManagerDelegate.DidAddService(CBPeripheralManager peripheral, CBService service, CBError error)
        {
            if (error != null)
            {
                Debug.LogError($"[DidAddService] error: {error}");
                return;
            }

            StartAdvertising();
        }

        void ICBPeripheralManagerDelegate.DidStartAdvertising(CBPeripheralManager peripheral, CBError error)
        {
            if (error != null)
            {
                Debug.LogError($"[DidStartAdvertising] error: {error}");
                return;
            }

            _header.SetStateText("Advertising...");
        }

        void ICBPeripheralManagerDelegate.DidReceiveWriteRequests(CBPeripheralManager peripheral, CBATTRequest[] requests)
        {
            var firstRequest = requests[0];
            foreach (var request in requests)
            {
                if (request.Characteristic.UUID != SampleLightControl_Data.LightControlCharacteristicUUID)
                {
                    peripheral.RespondToRequest(firstRequest, CBATTError.RequestNotSupported);
                    return;
                }

                if (request.Value.Length != 7)
                {
                    peripheral.RespondToRequest(firstRequest, CBATTError.InvalidAttributeValueLength);
                    return;
                }
            }

            var color = SampleLightControl_Data.ParseColor(firstRequest.Value);
            _light.color = color;
            peripheral.RespondToRequest(firstRequest, CBATTError.Success);
        }

        void OnAdvertiseToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                if (_peripheralManager.State != CBManagerState.PoweredOn)
                {
                    _advertiseToggle.isOn = false;
                    return;
                }
                if (_peripheralManager.IsAdvertising)
                {
                    return;
                }
                StartAdvertising();
            }
            else
            {
                _peripheralManager.StopAdvertising();
                _header.SetStateText("Stopped");
            }
        }

        void OnDestroy()
        {
            foreach (var d in _disposables)
            {
                d.Dispose();
            }
            _disposables.Clear();
        }
    }
}
