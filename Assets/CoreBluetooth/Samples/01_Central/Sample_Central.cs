using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreBluetooth;

namespace CoreBluetoothSample
{
    public class Sample_Central : MonoBehaviour, ICBCentralManagerDelegate, CBPeripheralDelegate
    {
        CBCentralManager centralManager;

        // See the following for generating UUIDs:
        // https://www.uuidgenerator.net/
        string serviceUUID = "068C47B7-FC04-4D47-975A-7952BE1A576F";

        void Start()
        {
            centralManager = CBCentralManager.Create(this);
        }

        public void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, int rssi)
        {
            Debug.Log($"[DiscoveredPeripheral] peripheral: {peripheral}  rssi: {rssi}");
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
            Debug.Log($"[DidDiscoverServices] peripheral: {peripheral}  error: {error}");
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
