# How To Extract Assets

由于模板只提供了夹馍的立绘，而从者立绘又在图片中，所以要么把图片下半部分抹除，要么提取对应的从者立绘作为底层图层。

所以在这里提供一个半成品工具用来一键提取。

或者自己搜索`AssetStudio`的使用方法

## 使用

![1.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/docs/imgs/1.jpg?raw=true)

首先点击`菜单->初始化`，等（界面从卡死中恢复后）出现`[CN] Initialized`之后，点击`菜单->下载AssetStorage`按钮，然后（界面从卡死中恢复后）点击`菜单->分析AssetStorage`，可以看到

![2.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/docs/imgs/2.jpg?raw=true)

点击`Assets`选项卡，搜索你要提取的立绘，比如

![3.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/docs/imgs/3.jpg?raw=true)

然后选中你要提取的资源，（因为bug或者我不会用，选择后会丢失最后一个被选中的项目，所以需要）按一下回车键，点击`下载->提取所选Assets`即可在`data/Assets`中找到。

`提取所选Assets`不会留下`.bin`文件，如果你需要可以先点击`下载所选Assets`

之后启动也是先点击`初始化`，然后`下载AssetStorage`、`分析AssetStorage`，但是如果已经下载过`AssetStorage`，可以跳过这一步。

只实现了提取图片和文本的功能，遇到剧情文件会自动提取解密后的文件。

因为是半成品，所以如果遇到任何bug可以**不用**告诉我。