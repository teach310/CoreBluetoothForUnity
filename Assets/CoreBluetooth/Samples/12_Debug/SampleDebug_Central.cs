using System.Text;
using CoreBluetooth;
using UnityEngine;

namespace CoreBluetoothSample
{
    public class SampleDebug_Central : MonoBehaviour, ICBCentralManagerDelegate, ICBPeripheralDelegate
    {
        // See the following for generating UUIDs:
        // https://www.uuidgenerator.net/
        string _serviceUUID = "068C47B7-FC04-4D47-975A-7952BE1A576F";
        string _characteristicUUID = "E3737B3F-A08D-405B-B32D-35A8F6C64C5D";

        CBCentralManager _centralManager;
        CBPeripheral _peripheral;
        CBCharacteristic _remoteCharacteristic;

        void Start()
        {
            _centralManager = new CBCentralManager(this);
        }

        public void DidDiscoverPeripheral(CBCentralManager central, CBPeripheral peripheral, int rssi)
        {
            Debug.Log($"[DidDiscoverPeripheral] peripheral: {peripheral}  rssi: {rssi}");
            _peripheral = peripheral;
            peripheral.Delegate = this;
            central.StopScan();
            central.Connect(peripheral);
        }

        public void DidUpdateState(CBCentralManager central)
        {
            Debug.Log($"[DidUpdateState] {central.State}");
            if (central.State == CBManagerState.PoweredOn)
            {
                Debug.Log($"[DidUpdateState] Start scanning for peripherals...");
                central.ScanForPeripherals(new string[] { _serviceUUID });
            }
        }

        public void DidConnectPeripheral(CBCentralManager central, CBPeripheral peripheral)
        {
            Debug.Log($"[DidConnectPeripheral] peripheral: {peripheral}");
            Debug.Log($"[DidConnectPeripheral] mtu: {peripheral.GetMaximumWriteValueLength(CBCharacteristicWriteType.WithResponse)}");
            peripheral.DiscoverServices(new string[] { _serviceUUID });
        }

        public void DidDisconnectPeripheral(CBCentralManager central, CBPeripheral peripheral, CBError error)
        {
            Debug.Log($"[DidDisconnectPeripheral] peripheral: {peripheral}  error: {error}");
        }

        public void DidFailToConnectPeripheral(CBCentralManager central, CBPeripheral peripheral, CBError error)
        {
            Debug.Log($"[DidFailToConnectPeripheral] peripheral: {peripheral}  error: {error}");
        }

        public void DidDiscoverServices(CBPeripheral peripheral, CBError error)
        {
            Debug.Log($"[DidDiscoverServices] peripheral: {peripheral}");
            if (error != null)
            {
                Debug.LogError($"[DidDiscoverServices] error: {error}");
                return;
            }

            foreach (var service in peripheral.Services)
            {
                Debug.Log($"[DidDiscoverServices] service: {service}, start discovering characteristics...");
                peripheral.DiscoverCharacteristics(new string[] { _characteristicUUID }, service);
            }
        }

        public void DidDiscoverCharacteristics(CBPeripheral peripheral, CBService service, CBError error)
        {
            Debug.Log($"[DidDiscoverCharacteristics] peripheral: {peripheral}  service: {service}");
            if (error != null)
            {
                Debug.LogError($"[DidDiscoverCharacteristics] error: {error}");
                return;
            }

            foreach (var characteristic in service.Characteristics)
            {
                Debug.Log($"[DidDiscoverCharacteristics] characteristic: {characteristic}");

                if (characteristic.UUID == _characteristicUUID)
                {
                    _remoteCharacteristic = characteristic;
                }

                if (characteristic.Properties.HasFlag(CBCharacteristicProperties.Notify))
                {
                    peripheral.SetNotifyValue(true, characteristic);
                }

                if (characteristic.Properties.HasFlag(CBCharacteristicProperties.Read))
                {
                    peripheral.ReadValue(characteristic);
                }
            }
        }

        public void DidUpdateValueForCharacteristic(CBPeripheral peripheral, CBCharacteristic characteristic, CBError error)
        {
            Debug.Log($"[DidUpdateValueForCharacteristic] characteristic: {characteristic}");
            if (error != null)
            {
                Debug.LogError($"[DidUpdateValueForCharacteristic] error: {error}");
                return;
            }

            var str = Encoding.UTF8.GetString(characteristic.Value);
            Debug.Log($"Data: {str}");
        }

        public void DidWriteValueForCharacteristic(CBPeripheral peripheral, CBCharacteristic characteristic, CBError error)
        {
            Debug.Log($"[DidWriteValueForCharacteristic] characteristic: {characteristic}");
            if (error != null)
            {
                Debug.LogError($"[DidWriteValueForCharacteristic] error: {error}");
                return;
            }
        }

        public void DidUpdateNotificationStateForCharacteristic(CBPeripheral peripheral, CBCharacteristic characteristic, CBError error)
        {
            Debug.Log($"[DidUpdateNotificationStateForCharacteristic] characteristic: {characteristic}");
            if (error != null)
            {
                Debug.LogError($"[DidUpdateNotificationStateForCharacteristic] error: {error}");
                return;
            }
        }

        public void OnClickWrite()
        {
            if (_peripheral == null)
            {
                Debug.Log("peripheral is null.");
                return;
            }

            if (_remoteCharacteristic == null)
            {
                Debug.Log("remoteCharacteristic is null.");
                return;
            }

            var value = UnityEngine.Random.Range(100, 1000).ToString();
            var data = Encoding.UTF8.GetBytes(value);
            _peripheral.WriteValue(data, _remoteCharacteristic, CBCharacteristicWriteType.WithResponse);
        }

        public void OnClickRead()
        {
            if (_peripheral == null)
            {
                Debug.Log("peripheral is null.");
                return;
            }

            if (_remoteCharacteristic == null)
            {
                Debug.Log("remoteCharacteristic is null.");
                return;
            }

            _peripheral.ReadValue(_remoteCharacteristic);
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
