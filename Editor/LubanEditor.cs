using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
#region
//保持UTF-8
#endregion
public class LubanEditor 
{
    [MenuItem("Luban/BuildLuban")]
    public static void LoadLuban()
    {
        string path = Application.dataPath;
        path = path.Remove(path.Length - 6);
        path = $"{path}Luban\\gen_code_json.bat";
        if (!File.Exists(path))
        {
            Debug.LogError($"请确保Luban文件夹存在于路径-->{path}");
            return;
        }

        Process proc = new Process();
        proc.StartInfo.FileName = path;
        //proc.StartInfo.Arguments = string.Format("10");//this is argument
        proc.StartInfo.UseShellExecute = true;//运行时隐藏dos窗口
        proc.StartInfo.CreateNoWindow = false;//运行时隐藏dos窗口
        proc.StartInfo.Verb = "runas";//设置该启动动作，会以管理员权限运行进程
        proc.Start();
        //proc.WaitForExit();

    }
}
