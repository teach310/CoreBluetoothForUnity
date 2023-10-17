using System;

namespace CoreBluetooth
{
    public abstract class CBManager
    {
        public CBManagerState State { get; protected set; } = CBManagerState.Unknown;
    }
}
