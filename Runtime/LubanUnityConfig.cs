using Codice.Client.Commands.Matcher;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/LubanUnityConfig")]
public class LubanUnityConfig : ScriptableObject
{


    [SerializeField,Header("生成资料文件的路径")]
    string lubanOutputDataPath;
    [SerializeField, Header("生成代码文件的路径")]
    string lubanOutputCodePath;

    [SerializeField, Header("运行的exe文件路径")]
    string lubanToolPath;
    [SerializeField, Header("存放Excel文件夹的路径")]
    string lubanExcelDataPath;
    [SerializeField, Header("配置定义文件路径")]
    string lubanDefinesPath;
    [SerializeField, Header("Root配置文件路径")]
    string lubanConfPath;
    [SerializeField, Header("检测Excel是否正常的exe")]
    string lubanCheckPath;
    [Space]
    //可以不定义的
    [SerializeField, Header("路径检测根文件路径")]
    string lubanPathValidatorRoot;
    [SerializeField, Header("多语言文件配置路径")]
    string lubanL10nTextProvider;

    public string LubanOutputDataPath { get => GetPath(lubanOutputDataPath); set => lubanOutputDataPath = value; }
    public string LubanOutputCodePath { get => GetPath(lubanOutputCodePath); set => lubanOutputCodePath = value; }
    public string LubanToolPath { get => GetPath(lubanToolPath); set => lubanToolPath = value; }
    public string LubanExcelDataPath { get => GetPath(lubanExcelDataPath); set => lubanExcelDataPath = value; }
    public string LubanDefinesPath { get => GetPath(lubanDefinesPath); set => lubanDefinesPath = value; }
    public string LubanConfPath { get => GetPath(lubanConfPath); set => lubanConfPath = value; }
    public string LubanCheckPath { get => GetPath(lubanCheckPath); set => lubanCheckPath = value; }
    public string LubanPathValidatorRoot { get => GetPath(lubanPathValidatorRoot); set => lubanPathValidatorRoot = value; }
    public string LubanL10nTextProvider { get => GetPath(lubanL10nTextProvider); set => lubanL10nTextProvider = value; }

    string GetPath(string path)
    {
        return Path.Combine(LubanUtil.ApplicationPath, path);
    }
    public void Init(string rootPath)
    {
        rootPath = Path.GetFullPath(rootPath);
        var unityRootPath=Path.GetFullPath(Application.dataPath);

        rootPath = rootPath.Replace(unityRootPath, "");
        unityRootPath = "";

        LubanToolPath = Path.Combine(rootPath, "Tools", "Luban", "Luban.exe");
        LubanExcelDataPath = Path.Combine(rootPath, "Config", "Datas");
        LubanDefinesPath = Path.Combine(rootPath, "Config", "Defines");
        LubanConfPath = Path.Combine(rootPath, "Config", "luban.conf");
        //这个之后也可以内嵌
        LubanCheckPath = Path.Combine(rootPath, "Config", "gen.bat");

        LubanOutputDataPath = Path.Combine(unityRootPath, "OutPutData");
        LubanOutputCodePath = Path.Combine(unityRootPath, "OutPutCode");
    }
}
