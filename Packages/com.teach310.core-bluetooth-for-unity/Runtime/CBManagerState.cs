namespace CoreBluetooth
{
    /// <summary>
    /// The possible states of a Core Bluetooth manager.
    /// https://developer.apple.com/documentation/corebluetooth/cbmanagerstate
    /// </summary>
    public enum CBManagerState
    {
        unknown = 0,
        resetting,
        unsupported,
        unauthorized,
        poweredOff,
        poweredOn
    }
}
