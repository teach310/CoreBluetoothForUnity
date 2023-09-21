namespace CoreBluetooth
{
    /// <summary>
    /// The possible states of a Core Bluetooth manager.
    /// https://developer.apple.com/documentation/corebluetooth/cbmanagerstate
    /// </summary>
    public enum CBManagerState
    {
        Unknown = 0,
        Resetting,
        Unsupported,
        Unauthorized,
        PoweredOff,
        PoweredOn
    }
}
