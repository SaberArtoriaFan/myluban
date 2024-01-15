using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
#region
//作者:Saber
#endregion
namespace Saber.Luban
{
    public class SOSingleton<T> : ScriptableObject where T:SOSingleton<T>,new ()
    {
        static T instance=null;
        public  static T Instance
        {
            get
            {
                if (instance == null)
                {
#if UNITY_EDITOR
                    if (Application.isPlaying)
                    {
                        InitRuntime();
                    }
                    else
                    {
                        InitEditor();
                    }
#else
                    InitRuntime();
#endif
                }
                return instance;
            }
        }
        public static string GetSavePath(bool isRuntime)
        {
            var t = typeof(T);
            foreach(var v in t.GetCustomAttributes(true))
            {
                if (v is AddressResourceAttribute ra)
                    return isRuntime ? ra.runtimePath : ra.editorPath;
            }
            return string.Empty;
        }
#if UNITY_EDITOR
        public static void InitEditor()
        {
            var path = GetSavePath(false);
            if (File.Exists(path) == false)
            {
                if (Directory.Exists(Path.GetDirectoryName(path)) == false)
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                AssetDatabase.CreateAsset(new T(), path);
                AssetDatabase.Refresh();
            }
            instance = AssetDatabase.LoadAssetAtPath<T>(path);
        }
#endif
        public static void InitRuntime()
        {
            var path = GetSavePath(true);
            instance = Resources.Load<T>(path);
        }

        public void Save()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.Refresh();
#endif
        }
    }
}
