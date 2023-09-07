using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreBluetooth
{
    public class CoreBluetoothForUnity
    {
        public int Hello()
        {
            Debug.Log("Hello from CoreBluetoothForUnity");
            return NativeMethods.cb4u_hello();
        }
    }
}
