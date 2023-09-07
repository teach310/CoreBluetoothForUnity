using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreBluetooth;

namespace CoreBluetoothSample
{
    public class Sample_Central : MonoBehaviour
    {
        void Start()
        {
            CoreBluetoothForUnity coreBluetooth = new CoreBluetoothForUnity();
            Debug.Log(coreBluetooth.Hello());
        }
    }
}
