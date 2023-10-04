namespace CoreBluetooth
{
    public class StartAdvertisingOptions
    {
        public string LocalName { get; set; } = null;
        public string[] ServiceUUIDs { get; set; } = null;

        public StartAdvertisingOptions()
        {
        }
    }
}
