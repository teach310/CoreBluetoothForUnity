using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreBluetooth;

namespace CoreBluetoothSample
{
    public class Sample_Central : MonoBehaviour, CBCentralManagerDelegate
    {
        CBCentralManager centralManager;

        // See the following for generating UUIDs:
        // https://www.uuidgenerator.net/
        string serviceUUID = "068C47B7-FC04-4D47-975A-7952BE1A576F";

        void Start()
        {
            centralManager = CBCentralManager.Create(this);
        }

        public void DidDiscoverPeripheral(CBCentralManager central, CBPeripheral peripheral, int rssi)
        {
            Debug.Log($"[DidDiscoverPeripheral] peripheral: {peripheral}  rssi: {rssi}");
            central.StopScan();
        }

        public void DidUpdateState(CBCentralManager central)
        {
            Debug.Log($"[DidUpdateState] {central.state}");
            if (central.state == CBManagerState.poweredOn)
            {
                central.ScanForPeripherals(new string[] { serviceUUID });
            }
        }
    }
}
