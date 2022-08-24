# FGOAssetReplace

这是一个`zygisk`模块，闭源，使用前考虑风险。

## 更新
- 添加中文字体，下载`Font`后扔在`Mod`文件夹内
- 添加从者说明，下载`mstSvtComment.yaml`后扔在`Mod`文件夹内
- 有小概率卡死在登录界面

![5.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/imgs/5.jpg?raw=true)

## 功能

- 替换Master头像、立绘
- 替换从者立绘
- 替换UI文本
- 替换剧情文本

## 预览
![1.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/imgs/1.jpg?raw=true)

因为使用了日服提取的资源，所以左下角是日文

![2.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/imgs/2.jpg?raw=true)


![3.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/imgs/3.jpg?raw=true)

缺少简体字所以看起来有点奇怪

![4.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/imgs/4.jpg?raw=true)

## 使用
模块的工作目录是`/sdcard/Android/data/包名/files/Mod/`，比如`/sdcard/Android/data/com.bilibili.fatego/files/Mod`或者`/storage/emulated/0/Android/data/com.aniplex.fategrandorder/files/Mod`

在`Release`中有预制的几个文件，其中：
- 为日服推荐下载的`LocalizationJpn.txt`
- 为日服推荐下载的`0805.script`
- 为国服推荐下载的`42.chara`
- 可选的`Master.chara`和`Custom.chara`

直接下载打包好的`*.chara`文件，放入工作目录下即可。

或者自己做，比较麻烦，需要自己`ps`处理图片，可以下载`psd`自己改。

创建`/sdcard/Android/data/包名/files/Mod/Figure/`文件夹，写一个`Figure.yaml`放在里面，内容如下：

```yaml
- name: abi
  enable: true
  id: "2500100"
- enable: true
  id: "2500200"
- name: kama
  enable: true
  id: "603700"
```

`name`是助记项可忽略，`id`为`从者id`，`enable`为`true`时启用。

然后创建`/sdcard/Android/data/包名/files/Mod/Figure/CharaGraph/`文件夹，放置详情页面立绘，按从者`id.png`方式命名，比如

`/sdcard/Android/data/包名/files/Mod/Figure/CharaGraph/603700.png`

窄边框的立绘同上，不过放置在`/sdcard/Android/data/包名/files/Mod/Figure/NarrowFigure/`文件夹下。

完整的文件树示例：

```shell
|---Figure
| |---Figure.yaml
| |---CharaGraph
| | |---2500100.png
| | |---603700.png
| |---NarrowFigure
| | |---2500100.png
| | |---603700.png
|
|---2b1b0ee6
|---0805.script
|---42.chara
|---LocalizationJpn.txt
```

