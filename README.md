<h1 align="center">
    MyLuban
</h1> 
<p align="center">基于Luban-Unity,主要完成了Luban工具的UnityPackage化.
  
<p align="center">完善中。。。

**主要作用**

1.方便Unity导入：打开Unity编辑器->PackageManager->Add package from git URL，填入本仓库URL即可；也可下载到本地添加。

2.自动化部署：点击Unity编辑器窗口 **Luban->InitLuban** 按钮,即可简单配置。
```
  **待完善**:Luban生成的CS脚本和Json文件路径可配置（目前固定）
    脚本路径->Assets\LubanLib
    Json文件路径->Assets\Resources
```
3.一键生成:点击Unity编辑器窗口 **Luban->BuildLuban** 按钮,即可自动根据 Luban(Assets同级目录)\Config\Datas\tables.xlsx 生成相应文件。

![Image](https://github.com/SaberArtoriaFan/pictures/blob/main/20231207-185623.jpg)

由于Unity网络问题，也可选择该gitee仓库[这是个仓库](https://gitee.com/Suzibuyi/my-luban)

更多关于Luban信息，请自行到Luban官方库学习。本仓库只是一个简单的将Luban嵌入Unity编辑器。
