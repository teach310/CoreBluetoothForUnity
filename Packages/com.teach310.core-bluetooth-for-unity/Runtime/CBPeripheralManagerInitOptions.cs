using CoreBluetooth.Foundation;

namespace CoreBluetooth
{
    public class CBPeripheralManagerInitOptions
    {
        public static readonly string ShowPowerAlertKey = "kCBInitOptionShowPowerAlert";
        public bool? ShowPowerAlert { get; set; } = null;

        public CBPeripheralManagerInitOptions()
        {
        }

        internal NSMutableDictionary ToNativeDictionary()
        {
            var dict = new NSMutableDictionary();
            if (ShowPowerAlert.HasValue)
            {
                using var value = new NSNumber(ShowPowerAlert.Value);
                dict.SetValue(ShowPowerAlertKey, value.Handle);
            }
            return dict;
        }
    }
}
