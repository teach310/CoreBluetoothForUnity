using UnityEngine;
using CoreBluetooth;

namespace CoreBluetoothSample
{
    public class Sample_Peripheral : MonoBehaviour, ICBPeripheralManagerDelegate
    {
        CBPeripheralManager peripheralManager;

        void Start()
        {
            peripheralManager = new CBPeripheralManager(this);
        }

        public void DidUpdateState(CBPeripheralManager peripheral)
        {
            Debug.Log($"[DidUpdateState] {peripheral.State}");
            if (peripheral.State == CBManagerState.PoweredOn)
            {
                Debug.Log($"[DidUpdateState] Start advertising...");
            }
        }

        void OnDestroy()
        {
            peripheralManager?.Dispose();
        }
    }
}
