using Codice.Utils;
using System;
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
    //[MenuItem("Luban/BuildLuban")]
    //public static void LoadLuban()
    //{
    //    string path = Application.dataPath;
    //    path = path.Remove(path.Length - 6);
    //    path = $"{path}Luban\\gen_code_json.bat";
    //    if (!File.Exists(path))
    //    {
    //        Debug.LogError($"请确保Luban文件夹存在于路径-->{path}");
    //        return;
    //    }

    //    Process proc = new Process();
    //    proc.StartInfo.FileName = path;
    //    //proc.StartInfo.Arguments = string.Format("10");//this is argument
    //    proc.StartInfo.UseShellExecute = true;//运行时隐藏dos窗口
    //    proc.StartInfo.CreateNoWindow = false;//运行时隐藏dos窗口
    //    proc.StartInfo.Verb = "runas";//设置该启动动作，会以管理员权限运行进程
    //    //EventHandler handler = null;
    //    //handler = (sender, e) => {
    //    //    AssetDatabase.Refresh();
    //    //    Debug.Log("Luban运行完毕");
    //    //    proc.Exited -= handler;
    //    //};
    //    //proc.Exited += handler;

    //    proc.Start();
    //    Debug.Log(@"Luban已在运行，出现`succ`字样后请 Ctrl+R 刷新AssetsData");
    //    //proc.WaitForExit();

    //}

    [MenuItem("Luban/InitLuban")]
    public static void InitLuban()
    {
        string path = "Packages/com.saber.luban";
        if(!Directory.Exists(path))
        {
            Debug.LogError($"不存在路径{path},请使用packageManager导入本包");
        }
        var dires=Directory.GetDirectories(path);
        //var files = Directory.GetFiles(path/*, "*.*", SearchOption.AllDirectories*/);
        foreach (var dir in dires)
        {
            var n = Path.GetFileName(dir);
            if (n== "MyLubanBuild~")
            {
                string p = Path.Combine(Application.dataPath.Remove(Application.dataPath.Length - ("/Assets").Length), "Luban");
                p = Path.GetFullPath(p);
                CopyFilesAndDirs(Path.GetFullPath(dir), p);
                break;
            }
            //var item = file.Split(new string[] { sourceProtocolName }, StringSplitOptions.RemoveEmptyEntries);
            //var newPath = $"{pluginsPath}{copyToProtocolName}/{item[1]}";
            //var path1 = Path.GetDirectoryName(newPath);
            //if (!Directory.Exists(path1))
            //    Directory.CreateDirectory(path1);
            //try { File.Copy(file, newPath, true); }
            //catch (Exception ex) { Debug.LogError(ex); }
        }
       // Debug.Log($"导入{Path.GetFileName(copyToProtocolName)}完成!");
        //AssetDatabase.Refresh();
    }
    static void CopyFilesAndDirs(string srcDir, string destDir)
    {
        if (!Directory.Exists(srcDir))
        {
            Debug.LogError("拷贝源文件夹不存在！！！" + srcDir);
            return;
        }
        bool isOn = false;
        bool isNew = true;
        if (!Directory.Exists(destDir))//若目标文件夹不存在
        {
            isOn = true;
        }
        else
        {
            if (EditorUtility.DisplayDialog("目录已经存在", "是否强制覆盖？\nConfig\\Datas下的内容将会全部保存", "是", "否"))
            {
                isOn = true;
                isNew = false;


                var dataPath = Path.Combine(destDir, "Config", "Datas");
                CopyDatasToTemp(dataPath);
                //var tempPath = Path.GetDirectoryName(destDir);
                //DirectoryInfo destDirInfo=new DirectoryInfo(destDir);
                //destDirInfo.MoveTo(tempPath);
                Directory.Delete(destDir,true);
            }
            else
            {
                return;
            }
        }

        if (isOn)
        {
            GetFilesAndDirs(srcDir, destDir,isNew);
            Debug.Log($"初始化完成！-->{destDir}");
        }
        if(isNew==false)
        {
            var tempPath = Path.Combine(Application.temporaryCachePath, "Luban", "Datas");
            if (Directory.Exists(tempPath)) 
                Directory.Delete(tempPath, true);
        }
    }
    static void CopyDatasToTemp(string dataPath)
    {
        var tempPath = Path.Combine(Application.temporaryCachePath, "Luban","Datas");
        if (Directory.Exists(tempPath)) Directory.Delete(tempPath,true);
        GetFilesAndDirs(dataPath, tempPath,true);
    }
    private static void GetFilesAndDirs(string srcDir, string destDir,bool isNew)
    {
        //原有数据，就不要覆盖了
        if (isNew == false&& Path.GetFileName(destDir) == "Datas" && Path.GetFileName(Path.GetDirectoryName(destDir)) == "Config")
        {
            var targetPath = Path.Combine(Application.temporaryCachePath, "Luban","Datas");
            targetPath=Path.GetFullPath(targetPath);
            if (!Directory.Exists(targetPath))
            {
                Debug.LogError($"存放资料的临时文件夹不见了,你或许去回收站还能找到!{targetPath}");
                return;
            }
            //DirectoryInfo dataInfo=new DirectoryInfo(targetPath);
            //dataInfo.MoveTo(destDir);
            Debug.Log("转移数据" + targetPath);
            //Directory.Delete(targetPath,true);
            //return;
            srcDir = targetPath;
            
        }

        string newPath;
        FileInfo fileInfo;
        Directory.CreateDirectory(destDir);//创建目标文件夹                                                  
        string[] files = Directory.GetFiles(srcDir);//获取源文件夹中的所有文件完整路径
        foreach (string path in files)          //遍历文件     
        {
            fileInfo = new FileInfo(path);
            newPath = Path.Combine(destDir, fileInfo.Name);
            Debug.Log($"复制:{path}-->{newPath}");
            File.Copy(path, newPath, true);
        }
        string[] dirs = Directory.GetDirectories(srcDir);
        foreach (string path in dirs)        //遍历文件夹
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            string newDir = Path.Combine(destDir, directory.Name);
            GetFilesAndDirs(path, newDir,isNew);
        }
    }
}
