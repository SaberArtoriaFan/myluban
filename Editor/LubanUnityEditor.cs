using Luban;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.AI;
using Debug = UnityEngine.Debug;

public class OutPutRecord
{
    public static readonly object check_lock=new object();

    public List<(bool,string)> list = new List<(bool, string)>();
    public int maxAutoShowTime = 1000;
    bool isShowed = false;
    int lastCheckTime;
    public OutPutRecord(int maxAutoShowTime = 1500)
    {
        this.maxAutoShowTime = maxAutoShowTime;
        lastCheckTime= 1500;
        AutoShow();
    }

    void AutoShow()
    {
        System.Timers.Timer myTimer = new System.Timers.Timer(1000); //实例化  设置时间间隔

        myTimer.Elapsed += new System.Timers.ElapsedEventHandler((o, e) =>
        {
            if (isShowed)
            {
                myTimer.Enabled = false;
                myTimer.Stop();
                lastCheckTime = 0;
                return;
            }
            lock (check_lock)
            {
                lastCheckTime -= 1000;
                if (lastCheckTime <= 0)
                {
                    Show();
                }
            }


        });  //给timer挂起事件 

        myTimer.AutoReset = true; //设置是执行一次（false）还是一直执行(true)； 

        myTimer.Enabled = true; //是否执行System.Timers.Timer.Elapsed事件
    }
    public void RecordOutPut(string s)
    {
        if (string.IsNullOrEmpty(s)) return;
        list.Add((false, s));
        lock (check_lock)
        {
            lastCheckTime = maxAutoShowTime;
        }
        if (isShowed)
            Debug.LogFormat($"<color=##00ff00>{s}</color>");
    }
    public void RecordError(string s)
    {
        if (string.IsNullOrEmpty(s)) return;
        list.Add((true,s));
        lock (check_lock)
        {
            lastCheckTime = maxAutoShowTime;
        }
        if (isShowed)
            Debug.LogFormat($"<color=#ff0000>{s}</color>");
    }

    internal void Show()
    {
        foreach(var v in list)
        {
            if (v.Item1)
                Debug.LogError(v.Item2);
            else
                Debug.Log(v.Item2);
        }
        //Debug.LogFormat(list.ToString());
        isShowed = true;
        
    }

    internal void Close()
    {
        list.Clear();
    }
}
public static class LubanUnityEditor 
{
    #region 常量定义，请直接修改
    public const string BuildBtn = "Tools/Luban/BuildData/Build";
    public const string BuildLightBtn = "Tools/Luban/BuildData/Build_Light";
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

    [MenuItem(BuildBtn, false, 1)]
    public static void Run()
    {
        Build(false);

    }
    [MenuItem(BuildLightBtn, false,2)]
    public static void RunLight()
    {
        Build(true);
    }


    static void Build(bool isLight)
    {
        var config = LoadConfig();
        if (config == null) return;
        //exe所在路径
        var path = config.LubanToolPath;
        if (File.Exists(path) == false)
        {
            Debug.LogError("不能找到Luban启动程序，请将编译后的LuBan文件放至" + path);
            return;
        }

        var process = new Process();
        process.StartInfo.FileName = path;
        if (isLight == false)
        {
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

        }


        var PATH_RootDir = Application.dataPath.Remove(Application.dataPath.Length - ("Assets").Length);

        var sb = new StringBuilder();
        sb.Append("-t" + " " + "all" + " ");
        sb.Append("-c" + " " + "cs-simple-json" + " ");
        sb.Append("-d" + " " + "json" + " ");
        //sb.Append(" ");
        sb.Append("--conf" + " " + $"\"{config.LubanConfPath}\"");
        sb.SetParas($"-x outputCodeDir=\"{config.LubanOutputCodePath}\"");
        sb.SetParas($"-x outputDataDir=\"{config.LubanOutputDataPath}\"");
        if (string.IsNullOrEmpty(config.LubanPathValidatorRoot) == false)
            sb.SetParas($"-x pathValidator.rootDir=\"{config.LubanPathValidatorRoot}\"");
        if (string.IsNullOrEmpty(config.LubanL10nTextProvider) == false)
            sb.SetParas($"-x l10n.textProviderFile=\"{config.LubanL10nTextProvider}\"");

        process.StartInfo.Arguments = sb.ToString();
        Debug.Log($"设置了参数-->{process.StartInfo.Arguments}");
        process.Start();
        if (isLight == false)
        {
            OutPutRecord outPutRecord = new OutPutRecord();
            EditorWindow.GetWindow<LubanBuildResultWindow>().Init(outPutRecord);
            process.OutputDataReceived += (o, e) => { outPutRecord.RecordOutPut(e.Data); };
            process.ErrorDataReceived += (o, e) => { outPutRecord.RecordError(e.Data); };
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            // Action action = () => AssetDatabase.Refresh();

            process.Exited += (o, e) =>
            {
                AssetDatabase.Refresh();
                outPutRecord.Show();
                outPutRecord.Close();
            };
        }
        else
        {
            process.Exited += (o, e) =>
            {
                AssetDatabase.Refresh();
            };
        }

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
