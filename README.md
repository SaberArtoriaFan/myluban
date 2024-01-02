+
<h1 align="center">
    MyLuban
</h1> 
<p align="center">基于Luban-Unity,主要完成了Luban工具的UnityPackage化.
  
<p align="center">完善中。。。

**主要作用**

1.方便Unity导入：打开Unity编辑器->PackageManager->Add package from git URL，填入本仓库URL即可；也可下载到本地添加。

2.自动化部署：点击Unity编辑器窗口 **Luban->InitLuban** 按钮,即可简单配置。

```
优点：

    1.Luban生成的CS脚本和Json文件路径以及Excel等资源文件路径可配置（比.bat文件的折磨好多了）

    2.操作简单化,一键生成，重要参数暴露，降低新人学习成本。

    3.便于导入，集成于Package包，一键导入。
    
```
3.一键生成:点击Unity编辑器窗口 **Luban->BuildLuban** 按钮,即可自动根据 Luban(Assets同级目录)\Config\Datas\tables.xlsx（默认配置可修改） 生成相应文件。

![Image](https://github.com/SaberArtoriaFan/pictures/blob/main/20240102-184747.jpg)

![Image](https://github.com/SaberArtoriaFan/pictures/blob/main/20231207-185623.jpg)

**更新日志**

2024/01/02：

    1.Menu按钮快速打开目标文件夹。
    
    2.暴露出主要参数（未来预计全部暴露）

    3.运行完毕后在Console界面打印日志，以及弹出Window显示日志

![Image](https://github.com/SaberArtoriaFan/pictures/blob/main/20240102-184330.jpg)

由于Unity网络问题，也可选择该gitee仓库[这是个仓库](https://gitee.com/Suzibuyi/my-luban)

更多关于Luban信息，请自行到[Luban官方](https://github.com/focus-creative-games/luban)学习。本仓库只是一个简单的将Luban嵌入Unity编辑器。
