# How To Extract Assets

由于模板只提供了夹馍的立绘，而从者立绘又在图片中，所以要么把图片下半部分抹除，要么提取对应的从者立绘作为底层图层。

所以在这里提供一个半成品工具用来一键提取。

或者自己搜索`AssetStudio`的使用方法

## 使用

![1.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/docs/imgs/1.jpg?raw=true)

首先点击`菜单->初始化`，等现`[CN] Initialized`之后，点击`菜单->下载AssetStorage`按钮，然后点击`菜单->分析AssetStorage`，可以看到

![2.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/docs/imgs/2.jpg?raw=true)

点击`Assets`选项卡，搜索你要提取的立绘，比如

![3.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/docs/imgs/3.jpg?raw=true)

然后选中你要提取的资源，由于选择后会丢失最后一个被选中的项目，所以需要按一下回车键，然后点击`下载->提取所选Assets`即可在`data/Assets`中找到。

直接`提取所选Assets`不会留下`.bin`文件，如果你需要`.bin`文件的话，可以先点击`下载所选Assets`，然后提取。

之后启动也是先点击`初始化`，然后`下载AssetStorage`、`分析AssetStorage`，但是如果已经下载过`AssetStorage`，可以跳过这一步。

只实现了`提取图片`和`文本`的功能，遇到剧情文件会自动提取解密后的文件。

因为是半成品，所以如果遇到任何bug可以**不用**告诉我。

## 其它
因为从`BGO`提取的文本是被和谐过，所以可以在仓库[FGOAssets](https://github.com/hexstr/FGOAssets)下载`ScriptReplaceRules.json`后，重命名为`ReplaceRules.json`放到`存放路径`中，默认是`X:\FGOAssetToolBox\data\ReplaceRules.json`

## 新增
可以导出模块需要的文件了，首先需要解析国服和日服的`MasterData`，然后点击`Conversion`标签，直接`导出`即可。  
当然，导出的文件是`.json`，你可以在[这个网站](https://codebeautify.org/json-to-yaml)完成转换

![4.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/docs/imgs/4.jpg?raw=true)