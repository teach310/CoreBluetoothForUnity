using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoreBluetoothSample
{
    public class SampleDebug_Home : MonoBehaviour
    {
        public void OnClickCentral()
        {
            SceneManager.LoadScene("SampleDebug_CentralScene");
        }

        public void OnClickPeripheral()
        {
            SceneManager.LoadScene("SampleDebug_PeripheralScene");
        }
    }
}

