using Codice.Client.Commands.Matcher;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/LubanUnityConfig")]
public class LubanUnityConfig : ScriptableObject
{
    

    [Header("生成资料文件的路径")]
    public string LubanOutputDataPath;
    [Header("生成代码文件的路径")]
    public string LubanOutputCodePath;

    [Header("运行的exe文件路径")]
    public string LubanToolPath;
    [Header("存放Excel文件夹的路径")]
    public string LubanExcelDataPath;
    [Header("配置定义文件路径")]
    public string LubanDefinesPath;
    [Header("Root配置文件路径")]
    public string LubanConfPath;
    [Header("检测Excel是否正常的exe")]
    public string LubanCheckPath;
    [Space]
    //可以不定义的
    [Header("路径检测根文件路径")]
    public string LubanPathValidatorRoot;
    [Header("多语言文件配置路径")]
    public string LubanL10nTextProvider;

    public void Init(string rootPath)
    {
        rootPath = Path.GetFullPath(rootPath);
        var unityRootPath=Path.GetFullPath(Application.dataPath);
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
