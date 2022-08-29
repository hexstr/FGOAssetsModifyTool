# Installation Guide

## 安装模块

### 下载地址

https://github.com/hexstr/FGOAssetsModifyTool/releases/tag/ModuleDownload

尽量在`Github`下载

### 手机上

直接下载并刷入`zygisk_FGOAssetReplace.zip`即可，一般下载最新版。

### 模拟器上

- 先刷入`zygisk_X86PluginLoader.zip`

- 再刷入`zygisk_Plugin_FGOAssetReplace.zip`

- 将`TargetListEmulator.json`放置于`/data/local/tmp/TargetListEmulator.json`

  因为`zygisk_Plugin_FGOAssetReplace.zip`只是复制文件，在`X86PluginLoader`生效后刷入`Plugin`可以不用重启模拟器。

因为模块只支持`arm64-v8a`，模拟器可能需要覆盖安装
````
adb install -r --abi arm64-v8a FateGO.apk
````

`FateGO.apk`替换成你下载的文件，比如`D:\Downloads\my-gwzdFateGO_v2.45.1_bili_706238.apk`

## 伪造libmain.so

如果你需要在·apk·中内置此模块，可以：

- 下载`zygisk_Plugin_FGOAssetReplace.zip`并解压
- 下载`libmain.so`
- 在目录`gh@hexstr\FGOAssetReplace`得到`arm64-v8a.so`
- 重命名为`libFGOAssetReplace.so`之后，和`libmain.so`一起塞到`FateGO.apk/lib/arm64-v8a`目录下
- 最后使用[MT管理器](https://www.coolapk.com/apk/bin.mt.plus)重新签名`FateGO.apk`并安装即可

当然如果你的设备有`root权限`但是没有`zygisk`也不想打包`apk`的话，直接把这两个文件`libmain.so`和`FGOAssetReplace.so`扔到`/data/app/com.aniplex.fategrandorder/lib/arm64`也可以

因为`BGO`有文件完整性校验，所以只支持`Fate/GO`。

## 导入资源文件

### 下载地址

https://github.com/hexstr/FGOAssetsModifyTool/releases/tag/AssetDownload

### 导入

第一次安装模块进入游戏后，会在`/sdcard/Android/data/[包名]/files/`文件夹下创建`Mod`文件夹，然后可以开始下载资源并导入了。

自己创建`Mod`文件夹可能会出现https://github.com/hexstr/FGOAssetsModifyTool/issues/19#issuecomment-1227482211

### 推荐下载

全部都是可选项

- 为日服推荐下载的`LocalizationJpn.txt`，对应`替换UI文本`
- 为日服推荐下载的`Font`，对应`替换中文字体`
- 为日服推荐下载的`数字.script`，对应`替换剧情文本`
- 为国服推荐下载的`数字.chara`，对应`替换从者立绘`
- 可选的`Master.chara`，对应`替换Master头像、立绘`
- 可选的`mstSvt.yaml`，对应`替换从者名称`
- 可选的`mstSvtComment.yaml`，对应`替换从者说明文本`

## 其它

**模块更新之后要手动删除`Mod`文件夹下的`2b1b0ee6`**

**模块更新之后要手动删除`Mod`文件夹下的`2b1b0ee6`**

**模块更新之后要手动删除`Mod`文件夹下的`2b1b0ee6`**