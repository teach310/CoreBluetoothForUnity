
using System;
using System.Collections.Generic;

namespace CoreBluetooth
{
    internal class ServiceLocator
    {
        public static ServiceLocator Instance { get; private set; } = new ServiceLocator();
        readonly Dictionary<Type, object> container = new Dictionary<Type, object>();

        public void Register<T>(T obj) where T : class
        {
            container[typeof(T)] = obj;
        }

        public bool Unregister<T>() where T : class
        {
            return container.Remove(typeof(T));
        }

        public T Resolve<T>() where T : class
        {
            Type targetType = typeof(T);
            if (container.TryGetValue(targetType, out object obj))
            {
                return obj as T;
            }

            return null;
        }
    }
}
