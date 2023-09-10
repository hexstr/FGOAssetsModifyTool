# FGOAssetReplace

酷安现在发言需要绑定中国大陆手机号了，以我的习惯是绝对不会绑定的，所以发在这里了:  
**年更模块[ModFGO](https://github.com/hexstr/ModFGO)更新了**

## 简介

这是一个`zygisk`模块，~~闭源~~核心部分已开源，使用前考虑风险。  
如果没有`root权限`，请参考`安装指南`中的`伪造libmain.so`部分

## 作用范围

模块会尝试在包含下列字符的进程中运行

- fategrandorder
- fatego
- bilibili.fgo
- com.tencent.tmgp.fgo

## 功能

- 替换Master头像、立绘
- 替换从者立绘
- 替换UI文本
- 替换剧情文本
- `从者名称`汉化
- `指令纹章名称`汉化
- `从者灵衣名称`汉化
- `Master服装名称`汉化
- `从者宝具名称`汉化
- `从者宝具描述`汉化
- `技能名称`汉化
- `技能描述`汉化
- `商店物品名称`汉化
- `任务名称`汉化（例如幕间物语中的任务）
- `战斗关卡名称`汉化（例如`大神殿`）
- `战斗地点名称`汉化（例如`太阳王的居城`）
- `物品名称`汉化（例如`圣晶石`）
- `Event名称`汉化（例如`活动加成从者友情点获得量2倍`，我也不知道显示在哪里）
- 感谢[chaldea-data](https://github.com/chaldea-center/chaldea-data/tree/main/mappings)提供的文本翻译（非自愿）

## 其它文档

- [安装指南](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/docs/InstallationGuide.md)
- [如何自定义立绘](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/docs/HowToCustomizeFigure.md)
- [如何一键提取立绘和文本](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/docs/HowToExtractAssets.md)

## 预览

~~彩蛋，为艾蕾替换主线中出现过的立绘，只需要把灵基设置为初始状态即可~~  
这个彩蛋已经移除

![1.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/imgs/1.jpg?raw=true)

因为使用了日服提取的资源，所以左下角是日文

![2.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/imgs/2.jpg?raw=true)

![3.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/imgs/3.jpg?raw=true)

![4.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/imgs/4.jpg?raw=true)

![5.jpg](https://github.com/hexstr/FGOAssetsModifyTool/blob/module/imgs/5.jpg?raw=true)

## 抓日志

终端模拟器输入

```shell
su
logcat --pid `pgrep fate`
```

或者下载[这个](https://f-droid.org/repo/com.dp.logcatapp_33.apk)，打开后搜索`hexstr`

如果日志不全的话，参考[这个](https://github.com/hexstr/FGOAssetsModifyTool/issues/106#issuecomment-1664838421)
