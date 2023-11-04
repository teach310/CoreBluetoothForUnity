using UnityEngine;

namespace CoreBluetooth
{
    internal class Initializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            ServiceLocator.Instance.Register<INativeCentralManagerCallbacks>(new NativeCentralManagerCallbacks());
            ServiceLocator.Instance.Register<INativePeripheralManagerCallbacks>(new NativePeripheralManagerCallbacks());
        }
    }
}
