# How To Customize Pictures

## 准备

- PhotoShop
- CharaGraph_603700.psd
- NarrowFigure_603700.psd
- Gudako.psd

## 说明

`CharaGraph`对应从者详情页面立绘

`NarrowFigure`对应编队时的窄边框立绘

## 尺寸

- `NarrowFigure`：宽 x 高 = 149 x 376
- `CharaGraph`：宽 x 高 = 512 x 725
- `Master头像`：256 x 256

## 制作

`NarrowFigure`和`CharaGraph`的`psd`文件都已经用参考线划分好了区域，裁剪成对应的尺寸直接覆盖上去就行。

因为一二破和三四破分别在两个文件中，所以你需要制作两个`png`文件。

`Master头像`裁剪对应的尺寸，重命名为`master.png`

`Master服装立绘`：在`psd`里自己看着位置叠加就行，可能要多次调整位置。导出的文件可以参考`Gudako_cover.png`和`Gudako_shadow.png`

## 导入

制作完成后，创建`Mod/Figure`、`Mod/Figure/NarrowFigure`和`Mod/Figure/CharaGraph`文件夹，把`master.png`、`Gudako_cover.png`和`Gudako_shadow.png`直接放到`Mod/Figure`文件夹就行。

从者立绘需要特别处理，首先写一个`Figure.yaml`，内容如下：

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

`name`是助记项可忽略，`id`为`从者id`，`enable`为`true`时启用。`从者id`可以在`mstSvt.json`找到。

把导出的两张`CharaGraph_603700.png`按从者`id[a|b].png`方式命名，也就是`603700a.png`和`603700b.png`。

把导出的`NarrowFigure_603700.png`重命名为`从者id.png`，也就是`603700.png`。

最后

- `Figure.yaml`放在`Mod`文件夹
- 把`603700a.png`和`603700b.png`放在`Mod/Figure/CharaGraph`文件夹
- 把`603700.png`放在`Mod/Figure/NarrowFigure`即可。

完整的文件树示例：

```shell
|---Figure.yaml(文件)
|
|---Figure(文件夹)
| |---Gudako_cover.png(文件)
| |---Gudako_shadow.png(文件)
| |---master.png(文件)
|
| |---CharaGraph(文件夹)
| | |---603700a.png(文件)
| | |---603700b.png(文件)
|
| |---NarrowFigure(文件夹)
| | |---603700.png(文件)
```

