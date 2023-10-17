namespace CoreBluetooth
{
    /// <summary>
    /// An error that Core Bluetooth returns while using Attribute Protocol (ATT).
    /// https://developer.apple.com/documentation/corebluetooth/cbatterror
    /// </summary>
    public enum CBATTError
    {
        Success = 0,
        InvalidHandle = 1,
        ReadNotPermitted = 2,
        WriteNotPermitted = 3,
        InvalidPdu = 4,
        InsufficientAuthentication = 5,
        RequestNotSupported = 6,
        InvalidOffset = 7,
        InsufficientAuthorization = 8,
        PrepareQueueFull = 9,
        AttributeNotFound = 10,
        AttributeNotLong = 11,
        InsufficientEncryptionKeySize = 12,
        InvalidAttributeValueLength = 13,
        UnlikelyError = 14,
        InsufficientEncryption = 15,
        UnsupportedGroupType = 16,
        InsufficientResources = 17,
    }
}
