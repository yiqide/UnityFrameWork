using System;
using System.Collections.Generic;
using System.Reflection;
using YiFramework.Interfase;
using UnityEngine;

namespace YiFramework.SingleMone
{
    public class SingleMonoManager : MonoBehaviour
    {
        public static SingleMonoManager Instance => initialized;
        private static SingleMonoManager initialized;

        private void Awake()
        {
            if (initialized != null) return;
            initialized = this;
            FindAddScript();
        }

        private static List<string> signType = new List<string>();

        public static void Sign(Type type)
        {
            if (!signType.Contains(type.FullName))
            {
                signType.Add(type.FullName);
            }
        }

        public static void Init()
        {
            if (initialized != null) return;
            GameObject go = new GameObject();
            go.AddComponent<SingleMonoManager>();
            go.name = "SingleMonoManager";
        }

        private static void FindAddScript()
        {
            DontDestroyOnLoad(initialized.gameObject);
            Dictionary<Type, Type[]> typeInterFaces = new Dictionary<Type, Type[]>();
            Assembly assembly = Assembly.GetAssembly(typeof(SingleMonoManager));
            foreach (var item in assembly.GetTypes())
            {
                if (item.IsClass && !item.IsAbstract) typeInterFaces.Add(item, item.GetInterfaces());
            }

            var singleMonoInterfaceTypes = initialized.GetTypes(typeInterFaces, typeof(ISingleMonoInterface));
            var singleInterfaceTypes = initialized.GetTypes(typeInterFaces, typeof(ISingleInterface));
            foreach (var item in singleMonoInterfaceTypes)
            {
                try
                {
                    if (!signType.Contains(item.FullName))
                    {
                        var attribute = item.GetCustomAttribute<SingleScriptAttribute>();
                        if (attribute == null)
                        {
                            ISingleMonoInterface sin = (ISingleMonoInterface)initialized.gameObject.AddComponent(item);
                            var methodInfo = sin.GetType()
                                .GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Instance);
                            methodInfo.Invoke(sin, null); //Invoke(sin,null);
                            Debug.Log("??????Mono?????????" + item.FullName);
                        }
                        else if (attribute.AutomaticallyCreated)
                        {
                            ISingleMonoInterface sin = (ISingleMonoInterface)initialized.gameObject.AddComponent(item);
                            var methodInfo = sin.GetType()
                                .GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Instance);
                            methodInfo.Invoke(sin, null); //Invoke(sin,null);
                            Debug.Log("??????Mono?????????" + item.FullName);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(new Exception(item.FullName + ":???????????????????????????SingleMonoBase<T>", e));
                    throw new Exception("????????????");
                }
            }

            foreach (var item in singleInterfaceTypes)
            {
                try
                {
                    if (!signType.Contains(item.FullName))
                    {
                        var attribute = item.GetCustomAttribute<SingleScriptAttribute>();
                        if (attribute == null)
                        {
                            ISingleInterface sin = (ISingleInterface)item.Assembly.CreateInstance(item.Name);
                            var methodInfo = sin.GetType()
                                .GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Instance);
                            methodInfo.Invoke(sin, null);
                            Debug.Log("?????????Mono?????????" + item.FullName);
                        }
                        else if (attribute.AutomaticallyCreated)
                        {
                            ISingleInterface sin = (ISingleInterface)item.Assembly.CreateInstance(item.Name);
                            var methodInfo = sin.GetType()
                                .GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Instance);
                            methodInfo.Invoke(sin, null);
                            Debug.Log("?????????Mono?????????" + item.FullName);
                        }
                        else
                        {
                            Debug.Log("????????????????????????" + item.FullName);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(new Exception(item.FullName + ":???????????????????????????SingleBase<T>", e));
                    throw new Exception("????????????");
                }
            }
        }

        public static void InstantiationMonoTarget<T>() where T : ISingleMonoInterface
        {
            ISingleMonoInterface sin = (ISingleMonoInterface)initialized.gameObject.AddComponent(typeof(T));
            var methodInfo = sin.GetType().GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Instance);
            methodInfo.Invoke(sin, null); //Invoke(sin,null);
            Debug.Log("??????Mono?????????" + typeof(T).FullName);
        }

        public static void InstantiationTarget<T>() where T : ISingleInterface
        {
            ISingleInterface sin = (ISingleInterface)typeof(T).Assembly.CreateInstance(typeof(T).FullName);
            Debug.Log(typeof(T).FullName);
            var methodInfo = sin.GetType().GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Instance);
            methodInfo.Invoke(sin, null);
            Debug.Log("?????????Mono?????????" + typeof(T).FullName);
        }

        private IEnumerable<Type> GetTypes(Dictionary<Type, Type[]> typeInterFaces, Type interfaceType)
        {
            foreach (var t in typeInterFaces)
            {
                foreach (var item in t.Value)
                {
                    if (item == interfaceType)
                    {
                        yield return t.Key;
                        break;
                    }
                }
            }
        }
    }
}