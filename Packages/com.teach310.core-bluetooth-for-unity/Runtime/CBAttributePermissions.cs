using System;

namespace CoreBluetooth
{
    [Flags]
    public enum CBAttributePermissions
    {
        Readable = 1,
        Writeable = 2,
        ReadEncryptionRequired = 4,
        WriteEncryptionRequired = 8
    }
}
