using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using YiFramework.Tools;

namespace NativeData
{
    [Serializable]
    public static class NativeDataContour
    {
        private static Dictionary<string, INativeInterface> allData = new Dictionary<string, INativeInterface>();
        
        
        public static T GetNativeData<T>(string fileName) where T:new()
        {
            string filePath = GetFilePath<T>(fileName);
            if (!allData.ContainsKey(fileName))
            {
                try
                {
                    if (File.Exists(filePath))
                    {
                        var data=SerializeTools.StringToObj<T>(FileTools.ReadFile(filePath));
                        allData.Add(filePath,(INativeInterface)data);
                        return data;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    var data = new T();
                    allData.Add(filePath,(INativeInterface)data);
                    return data;
                }
            }

            return (T)allData[filePath];
        }
        private static string GetFilePath<T>(string fileName)
        {
            return $"{Application.persistentDataPath}/{typeof(T).Name}{fileName}.json";
        }
        internal static void SaveData<T>(string filePath,T data)
        {
            try
            {
                FileTools.WriteFile(filePath,SerializeTools.ObjToString(data));
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
    internal interface INativeInterface
    {
        
    }
}
