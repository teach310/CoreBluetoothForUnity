using CoreBluetooth;
using UnityEngine;
using UnityEngine.UI;

namespace CoreBluetoothSample
{
    public class SampleLightControl_Central : MonoBehaviour, ICBCentralManagerDelegate, ICBPeripheralDelegate
    {
        [SerializeField] SampleLightControl_RGBSliders _rgbSliders;
        [SerializeField] Image _colorImage;
        [SerializeField] SampleLightControl_Header _header;

        CBCentralManager _centralManager;
        CBPeripheral _peripheral;
        CBCharacteristic _lightControlCharacteristic;

        Color32 _writeValue;
        bool _waitingForWrite = false;
        float _writeInterval = 0.1f;
        float _latestWriteTime = 0;

        void Start()
        {
            var initOptions = new CBCentralManagerInitOptions() { ShowPowerAlert = true };
            _centralManager = new CBCentralManager(this, initOptions);

            _rgbSliders.OnColorChanged.AddListener(OnColorChanged);
        }

        void Update()
        {
            if (_waitingForWrite)
            {
                if (Time.time - _latestWriteTime < _writeInterval) return;
                _latestWriteTime = Time.time;

                TurnLedOn(_writeValue);
                _waitingForWrite = false;
            }
        }

        void ICBCentralManagerDelegate.DidUpdateState(CBCentralManager central)
        {
            if (central.State == CBManagerState.PoweredOn)
            {
                _header.SetStateText("Scanning...");
                central.ScanForPeripherals(new string[] { SampleLightControl_Data.ServiceUUID });
            }
        }

        void ICBCentralManagerDelegate.DidDiscoverPeripheral(CBCentralManager central, CBPeripheral peripheral, int rssi)
        {
            _header.SetStateText("Connecting...");
            _peripheral = peripheral;
            peripheral.Delegate = this;
            central.StopScan();
            central.Connect(peripheral);
        }

        void ICBCentralManagerDelegate.DidConnectPeripheral(CBCentralManager central, CBPeripheral peripheral)
        {
            _header.SetStateText("Connected");
            peripheral.DiscoverServices(new string[] { SampleLightControl_Data.ServiceUUID });
        }

        void ICBCentralManagerDelegate.DidFailToConnectPeripheral(CBCentralManager central, CBPeripheral peripheral, CBError error)
        {
            _header.SetStateText("Scanning...");
            _lightControlCharacteristic = null;
            central.ScanForPeripherals(new string[] { SampleLightControl_Data.ServiceUUID });
        }

        void ICBCentralManagerDelegate.DidDisconnectPeripheral(CBCentralManager central, CBPeripheral peripheral, CBError error)
        {
            _header.SetStateText("Scanning...");
            _lightControlCharacteristic = null;
            central.ScanForPeripherals(new string[] { SampleLightControl_Data.ServiceUUID });
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
                peripheral.DiscoverCharacteristics(new string[] { SampleLightControl_Data.LightControlCharacteristicUUID }, service);
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
                if (characteristic.UUID == SampleLightControl_Data.LightControlCharacteristicUUID)
                {
                    _lightControlCharacteristic = characteristic;
                    _header.SetStateText("Ready");
                    return;
                }
            }
        }

        void ICBPeripheralDelegate.DidWriteValueForCharacteristic(CBPeripheral peripheral, CBCharacteristic characteristic, CBError error)
        {
            if (error != null)
            {
                Debug.LogError($"[DidWriteValueForCharacteristic] error: {error}");
                return;
            }
        }

        void TurnLedOn(Color32 color)
        {
            if (_lightControlCharacteristic == null) return;

            var data = SampleLightControl_Data.GetLedOnData(color);
            _peripheral.WriteValue(data, _lightControlCharacteristic, CBCharacteristicWriteType.WithResponse);
        }

        void OnColorChanged(Color32 color)
        {
            _colorImage.color = color;
            _writeValue = color;
            _waitingForWrite = true;
        }

        public void OnClickRandomColor()
        {
            if (_lightControlCharacteristic == null) return;

            var color = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), 255);
            _rgbSliders.SetColor(color);
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
