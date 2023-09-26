using UnityEngine;
using CoreBluetooth;
using System.Text;

namespace CoreBluetoothSample
{
    public class Sample_Central : MonoBehaviour, ICBCentralManagerDelegate, ICBPeripheralDelegate
    {
        CBCentralManager centralManager;

        // See the following for generating UUIDs:
        // https://www.uuidgenerator.net/
        string serviceUUID = "068C47B7-FC04-4D47-975A-7952BE1A576F";
        string characteristicUUID = "E3737B3F-A08D-405B-B32D-35A8F6C64C5D";
        CBPeripheral peripheral;
        CBCharacteristic remoteCharacteristic;

        void Start()
        {
            centralManager = CBCentralManager.Create(this);
        }

        public void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, int rssi)
        {
            Debug.Log($"[DiscoveredPeripheral] peripheral: {peripheral}  rssi: {rssi}");
            this.peripheral = peripheral;
            peripheral.Delegate = this;
            central.StopScan();
            central.Connect(peripheral);
        }

        public void UpdatedState(CBCentralManager central)
        {
            Debug.Log($"[UpdatedState] {central.State}");
            if (central.State == CBManagerState.PoweredOn)
            {
                Debug.Log($"[UpdatedState] Start scanning for peripherals...");
                central.ScanForPeripherals(new string[] { serviceUUID });
            }
        }

        public void ConnectedPeripheral(CBCentralManager central, CBPeripheral peripheral)
        {
            Debug.Log($"[ConnectedPeripheral] peripheral: {peripheral}");
            peripheral.DiscoverServices(new string[] { serviceUUID });
        }

        public void DisconnectedPeripheral(CBCentralManager central, CBPeripheral peripheral, CBError error)
        {
            Debug.Log($"[DisconnectedPeripheral] peripheral: {peripheral}  error: {error}");
        }

        public void FailedToConnect(CBCentralManager central, CBPeripheral peripheral, CBError error)
        {
            Debug.Log($"[FailedToConnect] peripheral: {peripheral}  error: {error}");
        }

        public void DiscoveredService(CBPeripheral peripheral, CBError error)
        {
            Debug.Log($"[DiscoveredService] peripheral: {peripheral}");
            if (error != null)
            {
                Debug.LogError($"[DiscoveredService] error: {error}");
                return;
            }

            foreach (var service in peripheral.Services)
            {
                Debug.Log($"[DiscoveredService] service: {service}, start discovering characteristics...");
                peripheral.DiscoverCharacteristics(new string[] { characteristicUUID }, service);
            }
        }

        public void DiscoveredCharacteristic(CBPeripheral peripheral, CBService service, CBError error)
        {
            Debug.Log($"[DiscoveredCharacteristic] peripheral: {peripheral}  service: {service}");
            if (error != null)
            {
                Debug.LogError($"[DiscoveredCharacteristic] error: {error}");
                return;
            }

            foreach (var characteristic in service.Characteristics)
            {
                Debug.Log($"[DiscoveredCharacteristic] characteristic: {characteristic}");

                if (characteristic.UUID == characteristicUUID)
                {
                    remoteCharacteristic = characteristic;
                }

                if (characteristic.Properties.HasFlag(CBCharacteristicProperties.Read))
                {
                    peripheral.ReadValue(characteristic);
                }
            }
        }

        public void UpdatedCharacteristicValue(CBPeripheral peripheral, CBCharacteristic characteristic, CBError error)
        {
            Debug.Log($"[UpdatedCharacteristicValue] characteristic: {characteristic}");
            if (error != null)
            {
                Debug.LogError($"[UpdatedCharacteristicValue] error: {error}");
                return;
            }

            var str = Encoding.UTF8.GetString(characteristic.Value);
            Debug.Log($"Data: {str}");
        }

        public void WroteCharacteristicValue(CBPeripheral peripheral, CBCharacteristic characteristic, CBError error)
        {
            Debug.Log($"[WroteCharacteristicValue] characteristic: {characteristic}");
            if (error != null)
            {
                Debug.LogError($"[WroteCharacteristicValue] error: {error}");
                return;
            }
        }

        public void OnClickWrite()
        {
            if (peripheral == null)
            {
                Debug.Log("peripheral is null.");
                return;
            }

            if (remoteCharacteristic == null)
            {
                Debug.Log("remoteCharacteristic is null.");
                return;
            }

            var value = UnityEngine.Random.Range(100, 1000).ToString();
            var data = Encoding.UTF8.GetBytes(value);
            peripheral.WriteValue(data, remoteCharacteristic, CBCharacteristicWriteType.WithResponse);
        }

        public void OnClickRead()
        {
            if (peripheral == null)
            {
                Debug.Log("peripheral is null.");
                return;
            }

            if (remoteCharacteristic == null)
            {
                Debug.Log("remoteCharacteristic is null.");
                return;
            }

            peripheral.ReadValue(remoteCharacteristic);
        }

        void OnDestroy()
        {
            if (centralManager != null)
            {
                centralManager.Dispose();
                centralManager = null;
            }
        }
    }
}
