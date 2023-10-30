using System;
using System.Collections.Generic;
using CoreBluetooth;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CoreBluetoothSample
{
    public class SampleButtonInformation_Peripheral : MonoBehaviour, ICBPeripheralManagerDelegate
    {
        [SerializeField] EventTrigger[] _buttons;

        [SerializeField] Text _stateLabel;

        CBPeripheralManager _peripheralManager;
        CBCentral _central;
        CBMutableCharacteristic _buttonInformationCharacteristic;

        List<IDisposable> _disposables = new List<IDisposable>();

        void Reset()
        {
            List<EventTrigger> buttonList = new List<EventTrigger>();
            buttonList.Add(GameObject.Find("Up").GetComponent<EventTrigger>());
            buttonList.Add(GameObject.Find("Right").GetComponent<EventTrigger>());
            buttonList.Add(GameObject.Find("Down").GetComponent<EventTrigger>());
            buttonList.Add(GameObject.Find("Left").GetComponent<EventTrigger>());
            buttonList.Add(GameObject.Find("A").GetComponent<EventTrigger>());
            buttonList.Add(GameObject.Find("B").GetComponent<EventTrigger>());
            _buttons = buttonList.ToArray();
        }

        void Start()
        {
            var initOptions = new CBPeripheralManagerInitOptions() { ShowPowerAlert = true };
            _peripheralManager = new CBPeripheralManager(this, initOptions);
            _disposables.Add(_peripheralManager);

            for (int i = 0; i < _buttons.Length; i++)
            {
                int buttonID = i + 1;
                var entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerDown;
                entry.callback.AddListener((data) => { OnButtonDown(buttonID); });
                _buttons[i].triggers.Add(entry);
                entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerUp;
                entry.callback.AddListener((data) => { OnButtonUp(buttonID); });
                _buttons[i].triggers.Add(entry);
            }
        }

        void StartAdvertising()
        {
            var options = new StartAdvertisingOptions()
            {
                ServiceUUIDs = new string[] { SampleButtonInformation_Data.ServiceUUID }
            };
            _peripheralManager.StartAdvertising(options);
        }

        void ICBPeripheralManagerDelegate.DidUpdateState(CBPeripheralManager peripheral)
        {
            if (peripheral.State == CBManagerState.PoweredOn && _buttonInformationCharacteristic == null)
            {
                var service = new CBMutableService(SampleButtonInformation_Data.ServiceUUID, true);
                _buttonInformationCharacteristic = new CBMutableCharacteristic(
                    SampleButtonInformation_Data.ButtonInformationCharacteristicUUID,
                    CBCharacteristicProperties.Read | CBCharacteristicProperties.Notify,
                    null,
                    CBAttributePermissions.Readable
                );
                service.Characteristics = new CBCharacteristic[] { _buttonInformationCharacteristic };
                _peripheralManager.AddService(service);
                _disposables.Add(service);
                _disposables.Add(_buttonInformationCharacteristic);
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

            _stateLabel.text = "Advertising...";
        }

        void ICBPeripheralManagerDelegate.DidSubscribeToCharacteristic(CBPeripheralManager peripheral, CBCentral central, CBCharacteristic characteristic)
        {
            if (_peripheralManager.IsAdvertising)
            {
                peripheral.StopAdvertising();
            }
            _stateLabel.text = "Connected";
            _central = central;
        }

        void ICBPeripheralManagerDelegate.DidUnsubscribeFromCharacteristic(CBPeripheralManager peripheral, CBCentral central, CBCharacteristic characteristic)
        {
            if (_peripheralManager.State == CBManagerState.PoweredOn && !_peripheralManager.IsAdvertising )
            {
                StartAdvertising();
            }
            _stateLabel.text = "Advertising...";
            _central = null;
        }

        void ICBPeripheralManagerDelegate.IsReadyToUpdateSubscribers(CBPeripheralManager peripheral)
        {
            Debug.Log($"[IsReadyToUpdateSubscribers] {peripheral}");
        }

        void OnButtonDown(int buttonID)
        {
            if (_central == null)
            {
                return;
            }
            UnityEngine.Debug.Log($"[OnButtonDown] buttonID: {buttonID}");

            var data = SampleButtonInformation_Data.GetButtonInformation(buttonID, true);
            _peripheralManager.UpdateValue(data, _buttonInformationCharacteristic, new CBCentral[] { _central });
        }

        void OnButtonUp(int buttonID)
        {
            if (_central == null)
            {
                return;
            }
            UnityEngine.Debug.Log($"[OnButtonUp] buttonID: {buttonID}");
            var data = SampleButtonInformation_Data.GetButtonInformation(buttonID, false);
            _peripheralManager.UpdateValue(data, _buttonInformationCharacteristic, new CBCentral[] { _central });
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