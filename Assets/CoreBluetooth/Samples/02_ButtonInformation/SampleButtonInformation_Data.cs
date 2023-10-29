using System;
using UnityEngine;

namespace CoreBluetoothSample
{
    public class SampleButtonInformation_Data
    {
        // See the following for generating UUIDs:
        // https://www.uuidgenerator.net/
        public static readonly string ServiceUUID = "068C47B7-FC04-4D47-975A-7952BE1A576F";
        public static readonly string ButtonInformationCharacteristicUUID = "E91E5ECB-A460-4DB1-97F7-F13D52222E15";

        /// <summary>
        /// Convert button information to byte array.
        /// This data format is same as toio button information.
        /// https://toio.github.io/toio-spec/en/docs/ble_button#read-operations
        /// </summary>
        /// <param name="isPressed"></param>
        /// <returns></returns>
        public static byte[] GetButtonInformationData(int buttonId, bool isPressed)
        {
            byte[] buff = new byte[2];
            buff[0] = BitConverter.GetBytes(Mathf.Clamp(buttonId, 0, 255))[0];
            buff[1] = (byte)(isPressed ? 0x80 : 0x00);
            return buff;
        }

        public static bool ParseButtonInformation(byte[] data, out int buttonId, out bool isPressed)
        {
            if (data.Length != 2)
            {
                buttonId = 0;
                isPressed = false;
                return false;
            }

            buttonId = data[0];
            isPressed = (data[1] & 0x80) != 0;
            return true;
        }
    }
}