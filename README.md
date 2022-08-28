# FGOAssetReplace

这是一个`zygisk`模块，闭源，使用前考虑风险。

## 功能

- 替换Master头像、立绘
- 替换从者立绘
- 替换UI文本
- 替换剧情文本
- 替换从者名称
- 替换从者说明文本

## 预览
![1.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/imgs/1.jpg?raw=true)

因为使用了日服提取的资源，所以左下角是日文

![2.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/imgs/2.jpg?raw=true)


![3.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/imgs/3.jpg?raw=true)

![4.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/imgs/4.jpg?raw=true)

## 使用
模块的工作目录是`/sdcard/Android/data/包名/files/Mod/`，比如`/sdcard/Android/data/com.bilibili.fatego/files/Mod`或者`/storage/emulated/0/Android/data/com.aniplex.fategrandorder/files/Mod`

在`Release`中有预制的几个文件，直接下载后放入工作目录下即可。

或者自己做，比较麻烦，需要自己`ps`处理图片，可以下载`psd`自己改。

写一个`Figure.yaml`放在`Mod`文件夹，内容如下：

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

然后创建`/sdcard/Android/data/包名/files/Mod/Figure/CharaGraph/`文件夹，放置详情页面立绘，按从者`id[a|b].png`方式命名，比如

`/sdcard/Android/data/包名/files/Mod/Figure/CharaGraph/603700a.png`

窄边框的立绘同上，不过放置在`/sdcard/Android/data/包名/files/Mod/Figure/NarrowFigure/`文件夹下。

`id`可以从`mstSvt.json`中获取

完整的文件树示例：

```shell
|---Figure.yaml
|---Figure
| |---CharaGraph
| | |---2500100a.png
| | |---603700a.png
| |---NarrowFigure
| | |---2500100.png
| | |---603700.png
|
|---2b1b0ee6
|---0805.script
|---42.chara
|---LocalizationJpn.txt
```

## 抓日志

终端模拟器输入

```shell
su
logcat -s hexstr:V
```

或者下载[这个](https://f-droid.org/repo/com.dp.logcatapp_33.apk)，打开后搜索`hexstr`
