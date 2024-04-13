# VRPROJECT 仓库
之前由于操作的不熟悉，有一个大文件在历史上被卡了，这个是最新的仓库，除了华容道没同步外，更新到展示前的最新版本，大家在重新拉一下这个新仓库把

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

# TODO:

#### 场景空间管理   **----饶文棋**

 **-[ ]依据现在的文档和博物馆模型先画一个场景布局设计草图(哪里放什么游戏，哪里放什么书，成就，哪里放人物照片等)，以防后面制作空间冲突**（ddl 下周一前）

  -[ ]博物馆布置（灯光，加玻璃，加装饰物，场景指引等）美化细节处理（可以参考一些其他虚拟博物馆的场景）

#### 模型  **----薛椀如**

  -[ ]游戏建模需求

  -[ ]找不到的一些必要模型需求等

#### 古代书籍和成就部分布置**----崔方博**

   -[ ]设置个按钮播放文字介绍[VR中医药博物馆_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1Hj411h7gw/?buvid=XX14AD219ED1171F64657CC38EBA4ACB91437&from_spmid=search.search-result.0.0&is_story_h5=false&mid=MzedknzF%2Fdz38Dbw3qizCQ%3D%3D&p=1&plat_id=116&share_from=ugc&share_medium=android&share_plat=android&share_session_id=82300f1f-2d77-4c45-bfa7-a987a158a29a&share_source=QQ&share_tag=s_i&spmid=united.player-video-detail.0.0&timestamp=1712213483&unique_k=tu3aWQJ&up_id=39686107&vd_source=da41ea704cfe3ccdc16a15beacfb00a9)可以参照这个的效果

   -[ ]某些有视频资源的可以设置按钮播放视频

#### 人物部分布置**----黄志恩**

   -[ ]从人物照片出发制作一些了解古代数学家的互动任务线

   -[ ]主菜单制作

#### 游戏部分布置**--郭寒阳**

   -[ ]算盘游戏

   -[ ]华容道完善一下

   -[ ]结束部分一个简单的问答游戏

   -[ ]（做完也许可以尝试一下3通的鲁班锁）

