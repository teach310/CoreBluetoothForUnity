using System;
using UnityEngine;

namespace CoreBluetoothSample
{
    public static class SampleLightControl_Data
    {
        // See the following for generating UUIDs:
        // https://www.uuidgenerator.net/
        public static readonly string ServiceUUID = "068C47B7-FC04-4D47-975A-7952BE1A576F";
        public static readonly string LightControlCharacteristicUUID = "2AE0B518-1DF2-4192-9E4A-EDEF2F30B04C";

        /// <summary>
        /// Convert color to byte array.
        /// This data format is same as toio light control.
        /// https://toio.github.io/toio-spec/en/docs/ble_light/#turning-the-indicator-on-and-off
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static byte[] GetLedOnData(Color32 color)
        {
            byte[] buff = new byte[7];
            buff[0] = 3;
            buff[1] = 0;
            buff[2] = 1;
            buff[3] = 1;
            buff[4] = BitConverter.GetBytes(color.r)[0];
            buff[5] = BitConverter.GetBytes(color.g)[0];
            buff[6] = BitConverter.GetBytes(color.b)[0];
            return buff;
        }

        public static Color32 ParseColor(byte[] data)
        {
            if (data.Length != 7) return Color.black;

            return new Color32(data[4], data[5], data[6], 255);
        }
    }
}
