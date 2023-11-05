using UnityEngine;

namespace CoreBluetooth
{
    internal class Initializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_IOS
            ServiceLocator.Instance.Register<INativeCentralManagerCallbacks>(new NativeCentralManagerCallbacks());
            ServiceLocator.Instance.Register<INativePeripheralManagerCallbacks>(new NativePeripheralManagerCallbacks());
            ServiceLocator.Instance.Register<INativePeripheralCallbacks>(new NativePeripheralCallbacks());
#endif
        }
    }
}
