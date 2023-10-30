using CoreBluetooth;
using UnityEngine;
using UnityEngine.UI;

namespace CoreBluetoothSample
{
    public class SampleButtonInformation_Central : MonoBehaviour, ICBCentralManagerDelegate, ICBPeripheralDelegate
    {
        [SerializeField] SampleButtonInformation_Log _log;
        [SerializeField] Text _stateLabel;

        CBCentralManager _centralManager;
        CBPeripheral _peripheral;

        void Start()
        {
            var initOptions = new CBCentralManagerInitOptions() { ShowPowerAlert = true };
            _centralManager = new CBCentralManager(this, initOptions);
        }

        void ICBCentralManagerDelegate.DidUpdateState(CBCentralManager central)
        {
            if (central.State == CBManagerState.PoweredOn)
            {
                _stateLabel.text = "Scanning...";
                central.ScanForPeripherals(new string[] { SampleButtonInformation_Data.ServiceUUID });
            }
        }

        void ICBCentralManagerDelegate.DidDiscoverPeripheral(CBCentralManager central, CBPeripheral peripheral, int rssi)
        {
            _stateLabel.text = "Connecting...";
            _peripheral = peripheral;
            peripheral.Delegate = this;
            central.StopScan();
            central.Connect(peripheral);
        }

        void ICBCentralManagerDelegate.DidConnectPeripheral(CBCentralManager central, CBPeripheral peripheral)
        {
            _stateLabel.text = "Connected";
            peripheral.DiscoverServices();
        }

        void ICBCentralManagerDelegate.DidFailToConnectPeripheral(CBCentralManager central, CBPeripheral peripheral, CBError error)
        {
            _stateLabel.text = "Scanning...";
            central.ScanForPeripherals(new string[] { SampleButtonInformation_Data.ServiceUUID });
        }

        void ICBCentralManagerDelegate.DidDisconnectPeripheral(CBCentralManager central, CBPeripheral peripheral, CBError error)
        {
            _stateLabel.text = "Scanning...";
            central.ScanForPeripherals(new string[] { SampleButtonInformation_Data.ServiceUUID });
        }

        void ICBPeripheralDelegate.DidDiscoverServices(CBPeripheral peripheral, CBError error)
        {
            if (error != null)
            {
                Debug.LogError($"[DidDiscoverServices] error: {error}");
                return;
            }
            foreach (var service in peripheral.Services)
            {
                if (service.UUID.Equals(SampleButtonInformation_Data.ServiceUUID))
                {
                    peripheral.DiscoverCharacteristics(new string[] { SampleButtonInformation_Data.ButtonInformationCharacteristicUUID }, service);
                }
            }
        }

        void ICBPeripheralDelegate.DidDiscoverCharacteristics(CBPeripheral peripheral, CBService service, CBError error)
        {
            if (error != null)
            {
                Debug.LogError($"[DidDiscoverCharacteristics] error: {error}");
                return;
            }
            foreach (var characteristic in service.Characteristics)
            {
                if (characteristic.UUID.Equals(SampleButtonInformation_Data.ButtonInformationCharacteristicUUID))
                {
                    if (characteristic.Properties.HasFlag(CBCharacteristicProperties.Notify))
                    {
                        peripheral.SetNotifyValue(true, characteristic);
                    }
                }
            }
        }

        void ICBPeripheralDelegate.DidUpdateValueForCharacteristic(CBPeripheral peripheral, CBCharacteristic characteristic, CBError error)
        {
            if (error != null)
            {
                Debug.LogError($"[DidUpdateValue] error: {error}");
                return;
            }

            if (SampleButtonInformation_Data.ParseButtonInformation(characteristic.Value, out int buttonId, out bool isPressed))
            {
                _log.AppendLog(buttonId, isPressed);
                Debug.Log($"[DidUpdateValue] buttonId: {buttonId}  isPressed: {isPressed}");
            }
        }

        void ICBPeripheralDelegate.DidUpdateNotificationStateForCharacteristic(CBPeripheral peripheral, CBCharacteristic characteristic, CBError error)
        {
            if (error != null)
            {
                Debug.LogError($"[DidUpdateNotificationState] error: {error}");
                return;
            }
            Debug.Log($"[DidUpdateNotificationState] characteristic: {characteristic}");
        }

        void OnDestroy()
        {
            if (_centralManager != null)
            {
                _centralManager.Dispose();
                _centralManager = null;
            }
        }
    }
}
