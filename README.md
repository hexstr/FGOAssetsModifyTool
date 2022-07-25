# FGOAssetsModifyTool
Fate/GO资源解密工具  
如果你需要的是在游戏中替换资源的功能，可以查看[这个分支](https://github.com/hexstr/FGOAssetsModifyTool/tree/module)

## 使用
新建一个文件夹`Android`把需要解密的资源扔进去；日服的资源文件所在目录为`/sdcard/Android/data/com.aniplex.fategrandorder/files/data/d713`  
* 1\~2: 加密&解密`bin`文件
* 3: 把`cfb1d36393fd67385e046b084b7cf7ed`重命名为`AssetStorage.txt`扔进`Android`
* 4: 需要先`解密AssetStorage.txt`，导出为`AudioName.json`和`AssetName.json`
* 5\~6: 把(比如`ScriptActionEncrypt@03.unity3d`)从`AssetStudio`中提取出来的文件加上后缀`.txt`并放在`Android/scripts`目录下(最好包含文件夹)，解密后的文本存放在`DecryptScripts/文件夹名`，重新加密后的文本存放在`EncryptScripts/文件夹名`  
* 7: 我自己汉化UI时用来快速替换汉化文本，一般不会用到  
* 8\~9: 下载游戏数据并解密
