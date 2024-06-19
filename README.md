# VRPROJECT 仓库


## 1.注意事项

### 开发环境

Unity 2022.3.22f1 lts

### 模拟器开发

这个地方记得取消勾选

![image-20240323181541583](https://xmuzhensimage.oss-cn-hangzhou.aliyuncs.com/image/image-20240323181541583.png)

然后确保simulator在场景内：
![image-20240323181640911](https://xmuzhensimage.oss-cn-hangzhou.aliyuncs.com/image/image-20240323181640911.png)

### Pico开发

注意把那个simulator删了,下面那个勾去掉

![image-20240323181727167](https://xmuzhensimage.oss-cn-hangzhou.aliyuncs.com/image/image-20240323181727167.png)

![image-20240413182011748](https://xmuzhensimage.oss-cn-hangzhou.aliyuncs.com/image/image-20240413182011748.png)

勾选livepriview

### 开发vr使用插件

unity interaction toolkit (可以查阅文档以及相关教程)



## 2.指南

### Git使用指南

git 图形化界面学习链接：https://www.bilibili.com/video/BV1bK411V7iq/?spm_id_from=333.337.search-card.all.click&vd_source=da41ea704cfe3ccdc16a15beacfb00a9
修改了Input.System为UnityEngine.Input，这样使用旧版脚本（2021）也可以。

### Unity下的git项目配置

https://zhuanlan.zhihu.com/p/57468011

### VR开发：

涉及vr交互目前基本都只是涉及unity interaction toolkit的使用，暂时没有用到pico的sdk，所以教程可以主要找interaction toolkit相关的

#### 1.交互：

这个预制体是就是玩家包含一些左右手的控制器等

![image-20240413182327894](https://xmuzhensimage.oss-cn-hangzhou.aliyuncs.com/image/image-20240413182327894.png)

canvas记得都先挂上下面这个

![image-20240413182805769](https://xmuzhensimage.oss-cn-hangzhou.aliyuncs.com/image/image-20240413182805769.png)

可以被交互的图片要参照祖冲之image做一下相应的设置，相应的交互就在interacterble设置就行（有点像buttom那种的设置）：

#### 2.UI

可以用导入的moderUIpack去做较为好看的UI

![image-20240413183613809](https://xmuzhensimage.oss-cn-hangzhou.aliyuncs.com/image/image-20240413183613809.png)

#### 3.DOTween

用这个可以不用动画制作一些渐变的交互效果

也可以直接照搬这个脚本

![image-20240413193455841](https://xmuzhensimage.oss-cn-hangzhou.aliyuncs.com/image/image-20240413193455841.png)


