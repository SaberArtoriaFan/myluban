using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.AI;
using Debug = UnityEngine.Debug;

public class OutPutRecord
{
    public StringBuilder list=new StringBuilder ();
    
    public void RecordOutPut(string s)
    {
        if (string.IsNullOrEmpty(s)) return;
        list.AppendLine(s);
    }
    public void RecordError(string s)
    {
        if (string.IsNullOrEmpty(s)) return;
        list.AppendLine($"<color=#ff0000>{s}</color>");
    }

    internal void Show()
    {
        Debug.Log(list.ToString());
        
    }

    internal void Close()
    {
        list.Clear();
    }
}
public static class LubanUnityEditor 
{
    #region 常量定义，请直接修改
    public const string WindowBtn = "Tools/Luban/BuildData";
    public const string ConfigPath = "LubanConfig.asset";

    public static string ApplicationPath = Path.GetFullPath(Application.dataPath.Remove(Application.dataPath.Length - ("/Assets").Length));
    public static string LubanRootPath =Path.GetFullPath(Path.Combine(Application.dataPath.Remove(Application.dataPath.Length - ("/Assets").Length),"Luban"));
    #endregion

    public const string PATH_LubanConf = "";
    public const string PATH_OutCodeDir = "";
    public const string PATH_OutDataDir = "";
    //public const string PATH_RootDir = "";
    public const string PATH_TextProviderFile = "";

    public static void SetParas(this StringBuilder sb, string para)
    {
        if (sb.Length > 0)
            sb.Append(" ");
        //sb.Append($"\"{para}\"");
        sb.Append(para);

    }

    [MenuItem(WindowBtn, false, 1)]
    public static void Run()
    {
        var config = LoadConfig();
        if(config == null) return;
        //exe所在路径
        var path = config.LubanToolPath;
        var process = new Process();
        process.StartInfo.FileName = path;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;

       
        var PATH_RootDir = Application.dataPath.Remove(Application.dataPath.Length - ("Assets").Length);

        var sb = new StringBuilder();
        sb.Append("-t"+" "+"all"+" ");
        sb.Append("-c"+" "+"cs-simple-json" + " ");
        sb.Append("-d"+" "+"json"+" ");
        //sb.Append(" ");
        sb.Append("--conf"+" "+$"\"{config.LubanConfPath}\"");
        sb.SetParas($"-x outputCodeDir=\"{config.LubanOutputCodePath}\"");
        sb.SetParas($"-x outputDataDir=\"{config.LubanOutputDataPath}\"");
        if(string.IsNullOrEmpty(config.LubanPathValidatorRoot)==false)
            sb.SetParas($"-x pathValidator.rootDir=\"{config.LubanPathValidatorRoot}\"");
        if(string.IsNullOrEmpty(config.LubanL10nTextProvider)==false)
            sb.SetParas($"-x l10n.textProviderFile=\"{config.LubanL10nTextProvider}\"");

        process.StartInfo.Arguments = sb.ToString();
        Debug.Log($"设置了参数-->{process.StartInfo.Arguments}");
        OutPutRecord outPutRecord = new OutPutRecord();
        process.Start();
        process.OutputDataReceived += (o,e)=> { outPutRecord.RecordOutPut(e.Data); };
        process.ErrorDataReceived += (o, e) => { outPutRecord.RecordError(e.Data); };
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
       // Action action = () => AssetDatabase.Refresh();

        process.Exited += (o, e) => {
            AssetDatabase.Refresh();
            outPutRecord.Show();
            outPutRecord.Close();
        };
        process.EnableRaisingEvents = true;
        process.WaitForExit();
    }

    public  static LubanUnityConfig LoadConfig()
    {
        var path = Path.Combine(Application.dataPath, ConfigPath);
        path=Path.GetFullPath(path);
        if (Directory.Exists(Path.GetDirectoryName(path)) == false) 
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        LubanUnityConfig config = null;
        if(File.Exists(path))
            config = AssetDatabase.LoadAssetAtPath<LubanUnityConfig>(Path.Combine("Assets",ConfigPath));
        if (config == null)
        {
            Debug.Log($"未检测到配置文件{path},已自动生成");
           
            config = LubanUnityConfig.CreateInstance<LubanUnityConfig>();
            config.Init(LubanRootPath);
            AssetDatabase.CreateAsset(config, Path.Combine("Assets", ConfigPath));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        //检测各路径是否正确
        var type=config.GetType();
        var fields=type.GetFields(System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.Instance);
        bool isError = false;
        foreach(var field in fields)
        {
            if (field.Name.StartsWith("Luban") == false) continue;
            var value = field.GetValue(config);
            if (value is string sv&&string.IsNullOrEmpty(sv)==false)
            {
                if(Path.HasExtension(sv))
                {
                    if (!File.Exists(sv))
                    {
                        isError = true;
                        Debug.LogError($"路径不存在-->{sv}");
                    }
                }
                else
                {
                    if(!Directory.Exists(sv))
                    {
                        if (field.Name.Contains("Output"))
                            Directory.CreateDirectory(sv);
                        else
                        {
                            isError = true;
                            Debug.LogError($"路径不存在-->{sv}");
                        }
                    }
                }
            }
        }
        AssetDatabase.Refresh();
        if(isError)
            Debug.LogError($"程序存在错误!!!,请检查是否[Luban/InitLuban],并验证文件完整性");

        return isError?null:config;
    }
}
