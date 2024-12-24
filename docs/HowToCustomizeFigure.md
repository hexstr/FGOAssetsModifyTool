# How To Customize Pictures

## 准备

- PhotoShop
- CharaGraph_603700.psd
- NarrowFigure_603700.psd
- CharaFigure_3032002.psd
- Gudako.psd
- 303200.png

## 说明

`CharaGraph`对应从者详情页面立绘

`NarrowFigure`对应编队时的窄边框立绘

`CharaFigure`对应剧情和主页立绘

`Status`对应战斗时的半身像

`Servants`对应从者模型贴图

## 尺寸

- `NarrowFigure`：宽 x 高 = 149 x 376
- `CharaGraph`：宽 x 高 = 512 x 725
- `CharaFigure`：最大宽高 1024 x 1024，需要自己调整
- `Status`：宽 x 高 = 256 x 256
- `Master头像`：256 x 256
- `Servants`：需要提取原始贴图然后修改

## 制作

`NarrowFigure`和`CharaGraph`的`psd`文件都已经用参考线划分好了区域，裁剪成对应的尺寸直接覆盖上去就行。

因为一二破和三四破分别在两个文件中，所以你需要制作两个`png`文件。

`Master头像`裁剪对应的尺寸，重命名为`master.png`。

`Status`自己看着办，要三张，参考`Status_603700_[1|2|3].png`。

`Master服装立绘`：在`psd`里自己看着位置叠加就行，可能要多次调整位置。导出的文件可以参考`Gudako_cover.png`和`Gudako_shadow.png`。

`CharaFigure`需要原始图片和黑色剪影两张图片，文件可以参考`3032002.png`和`3032002a.png`

`Servants`需要提取原始贴图然后修改，文件可以参考`303200.png`

## 导入

制作完成后，创建`Mod/Figure`、`Mod/Figure/Status`、`Mod/Figure/NarrowFigure`、`Mod/Figure/CharaGraph`和`Mod/Figure/CharaFigure`文件夹，把`master.png`、`Gudako_cover.png`和`Gudako_shadow.png`直接放到`Mod/Figure`文件夹就行。

把导出的两张`CharaGraph_603700.png`按从者`id[a|b].png`方式命名，也就是`603700a.png`和`603700b.png`。

把导出的`NarrowFigure_603700.png`重命名为`从者id.png`，也就是`603700.png`。

把三张`Status`重命名为`从者id_[1|2|3].png`，也就是`603700_1.png`、`603700_2.png`和`603700_3.png`。

最后

- 把`603700a.png`和`603700b.png`放在`Mod/Figure/CharaGraph`文件夹
- 把`603700_1.png`、`603700_2.png`和`603700_3.png`放在`Mod/Figure/Status`文件夹
- 把`603700.png`放在`Mod/Figure/NarrowFigure`文件夹
- 把`3032002.png`和`3032002a.png`放在`Mod/Figure/CharaFigure`文件夹
- 把`303200.png`放在`Mod/Figure/Servants`文件夹即可。

完整的文件树示例：

```shell
|---Figure(文件夹)
| |---Gudako_cover.png(文件)
| |---Gudako_shadow.png(文件)
| |---master.png(文件)
|
| |---CharaGraph(文件夹)
| | |---3032002.png(文件)
| | |---3032002a.png(文件)
|
| |---CharaFigure(文件夹)
| | |---603700a.png(文件)
| | |---603700b.png(文件)
|
| |---NarrowFigure(文件夹)
| | |---603700.png(文件)
|
| |---Servants(文件夹)
| | |---303200.png(文件)
|
| |---Status(文件夹)
| | |---603700_1.png(文件)
| | |---603700_2.png(文件)
| | |---603700_3.png(文件)
```
