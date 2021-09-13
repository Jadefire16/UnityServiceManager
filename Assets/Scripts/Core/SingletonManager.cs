using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace JadesToolkit
{
    public static class SingletonManager
    {
        private static Dictionary<System.Type, MonoSingleton> dependencies = new Dictionary<Type, MonoSingleton>();

        private static GameObject coreObject;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialize()
        {
            dependencies.Clear();
            coreObject = new GameObject("Dependencies Manager");
            Assembly assembly = Assembly.GetAssembly(typeof(MonoSingleton)); // fetch all MonoSingleton types

            var allDependencyTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(MonoSingleton)) && t.IsAbstract == false).ToArray();

            foreach (var dependency in allDependencyTypes)
            {
                var component = coreObject.AddComponent(dependency) as MonoSingleton;
                component.Initialize();
                dependencies.Add(dependency, component);
            }
            UnityEngine.Object.DontDestroyOnLoad(coreObject);
        }

        public static bool TryGetDependency<T>(out T value) where T : MonoSingleton
        {
            value = null;
            bool success = dependencies.TryGetValue(typeof(T), out MonoSingleton val);
            if (!success)
                return false;
            value = val as T;
            return true;
        }

    }
}