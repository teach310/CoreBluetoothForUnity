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

        public void DidUpdateState(CBCentralManager central)
        {
            if (central.State == CBManagerState.PoweredOn)
            {
                _stateLabel.text = "Scanning...";
                central.ScanForPeripherals(new string[] { SampleButtonInformation_Data.ServiceUUID });
            }
        }

        public void DidDiscoverPeripheral(CBCentralManager central, CBPeripheral peripheral, int rssi)
        {
            _stateLabel.text = "Connecting...";
            _peripheral = peripheral;
            peripheral.Delegate = this;
            central.StopScan();
            central.Connect(peripheral);
        }

        public void DidConnectPeripheral(CBCentralManager central, CBPeripheral peripheral)
        {
            _stateLabel.text = "Connected";
            peripheral.DiscoverServices();
        }

        public void DidFailToConnectPeripheral(CBCentralManager central, CBPeripheral peripheral, CBError error)
        {
            _stateLabel.text = "Scanning...";
            central.ScanForPeripherals(new string[] { SampleButtonInformation_Data.ServiceUUID });
        }

        public void DidDisconnectPeripheral(CBCentralManager central, CBPeripheral peripheral, CBError error)
        {
            _stateLabel.text = "Scanning...";
            central.ScanForPeripherals(new string[] { SampleButtonInformation_Data.ServiceUUID });
        }

        public void DidDiscoverServices(CBPeripheral peripheral, CBError error)
        {
            if (error != null)
            {
                Debug.LogError($"[DidDiscoverServices] error: {error}");
                return;
            }
            foreach (var service in peripheral.Services)
            {
                peripheral.DiscoverCharacteristics(new string[] { SampleButtonInformation_Data.ButtonInformationCharacteristicUUID }, service);
            }
        }

        public void DidDiscoverCharacteristics(CBPeripheral peripheral, CBService service, CBError error)
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

        public void DidUpdateValue(CBPeripheral peripheral, CBCharacteristic characteristic, CBError error)
        {
            if (error != null)
            {
                Debug.LogError($"[DidUpdateValue] error: {error}");
                return;
            }

            if (SampleButtonInformation_Data.ParseButtonInformation(characteristic.Value, out int buttonId, out bool isPressed))
            {
                _log.AppendLog(buttonId);
            }
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
