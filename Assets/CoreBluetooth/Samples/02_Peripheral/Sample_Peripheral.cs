using UnityEngine;
using CoreBluetooth;

namespace CoreBluetoothSample
{
    public class Sample_Peripheral : MonoBehaviour, ICBPeripheralManagerDelegate
    {
        CBPeripheralManager peripheralManager;

        void Start()
        {
            peripheralManager = CBPeripheralManager.Create(this);
        }

        public void DidUpdateState(CBPeripheralManager peripheral)
        {
            Debug.Log($"[DidUpdateState] {peripheral.State}");
            if (peripheral.State == CBManagerState.PoweredOn)
            {
                Debug.Log($"[DidUpdateState] Start advertising...");
                // peripheral.StartAdvertising(new string[] { "068C47B7-FC04-4D47-975A-7952BE1A576F" });
            }
        }

        void OnDestroy()
        {
            peripheralManager?.Dispose();
        }
    }
}
