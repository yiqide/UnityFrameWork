using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Tilemaps;
using UnityEngine;
using YiFramework.Tools;

namespace NativeData
{
    [Serializable]
    public abstract class NativeDataInstance<T> : INativeInterface where T : NativeDataInstance<T>,new()
    {
        public string FilePath =>$"{Application.persistentDataPath}/{typeof(T).Name}{FileName}.json";
        public abstract string FileName { get; }
        
        public void Save()
        {
            OnSaveBefore();
            NativeDataContour.SaveData<T>(FilePath,(T)this);
            OnSaveAfter();
        }

        public virtual void OnSaveBefore()
        {
            
        }
        
        public virtual void OnSaveAfter()
        {
            
        }
    }
}
