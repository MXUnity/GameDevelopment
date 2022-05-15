

# 	Pipeline

##### 渲染管线

​	指渲染数据渲染到屏幕的过程，一般模型数据到屏幕需要经过以下过程：

​	CPU阶段(准备数据)->几何阶段(顶点数据换算到屏幕空间,准备光栅前数据)->着色阶段（光栅化到写入屏幕像素点）

​	分细过程其实就是：

​	数据准备然后调用DrawCall->模型空间->世界空间->裁剪空间->屏幕空间 (这后面还有曲面细分等可操作阶段)->光栅化(三维数据转二维)->着色计算->深度测试->	透明测试->模板测试->写入屏幕backframebuffer

​	以上是一般流程

##### 管线主流

​	现代主线主流渲染管线分为Forward,Deffered两种，也可以自定义一些渲染管线

###### Forward/Forward+

​	Link : https://github.com/MXUnity/GameDevelopment/tree/main/Assets/Pipeline/Forward

###### Deffered Rendering

​	Link : https://github.com/MXUnity/GameDevelopment/tree/main/Assets/Pipeline/Deferred

#### GPU硬件架构：

​	事实上渲染管线是一个概念上或者说程序使用上的的流程，在程序运行在GPU上的时候，GPU的架构的不一样对同一个程序渲染的消耗完全不同结果

实际开发需要针对特点环境去对程序进行优化，现在市面上主流有以下两种IMR和TBR/TBDR，Android/iOS主流使用TBR/TBDR的GPU架构，事实上GPU架构

在不同厂商用不一样技术导致实际优化方向不一样。https://zhuanlan.zhihu.com/p/482448457

例如：高通提出了FlexRender的渲染架构，就可以在运行时候自动切换IMR和TBR渲染模式，在后处理多批次速度快于单纯使用TBR架构(实际上在测试华为和小米

在bloom性能上优化上差异发现的问题，减少运算复杂度对小米优化有效果对华为没有效果，减少批次对华为有效果对小米优化效果不明显)

参考:

IMR/TBR详解：

https://zhuanlan.zhihu.com/p/112120206

https://blog.csdn.net/zju_fish1996/article/details/109269448

GPU厂商硬件架构技术：

https://www.sohu.com/a/83561143_119711

https://news.mydrivers.com/1/266/266555_all.htm

##### IMR

​		立即渲染的GPU渲染架构，基本在一个Drawcall指令下会计算出渲染结果，每次执行都会经过以下流程

![IMR-Pipeline-1](README/IMR-Pipeline-1.jpg)



##### TBDR/TBR

​	移动端或者带电源的主机一般都是使用TBR渲染架构，为了减少带宽消耗，减少功耗，通过先缓存顶点数据，通过光栅化前做一次Early-z(进行一次顶点遮挡剔

除)，减少实际在着色阶段运算量以及会系统内存读写数量，通过小片段的On-Chip来缓存数据，可以提高读写速度和减少能耗

TBDR是PowerVR(苹果所使用的GPU)提出申请专利的，实际是TBR的优化而来， 这个D其实是增加了下面光栅化后的深度剔除，顶点剔除后还存在重叠部分

overdraw，光栅化后的剔除能保证精确去除overdraw，把着色阶段运算量降到最低。

![TBDR-Pipeline-1](README/TBDR-Pipeline-1.jpg)

##### 总结

由于上面GPU架构区别可以得出以下结论：

###### TBR/TBDR总结

带电源体积小的移动设备因为体积和能耗问题使用TBR架构，那么同时带来的问题就有

1：顶点压力，因为要光栅前要缓存顶点数据切片作为后面剔除的依据，读写过程产生带宽占用，所以移动设备同屏顶点数最好不要超过20W，有人测试100w顶点数就算高端移动（2021年）设备也会触发瓶颈

2：MSAA以及延迟渲染管线都会给TBR带来比较大的压力，因为数据量变大使得On-Chip能缓存的量就缩水，会导致比不做msaa的向前渲染管线切片更多，带宽压力大，同时也导致一个单位时间GPU需要处理的切片数变多了，大大提高了一帧内GPU的耗时

3：体积小也就带着能带的运算逻辑单元少，处理性能不如主机PC运算，有时候读图的带宽压力比运算低，在PC或主机可以用运算替代读图减少带宽，真机有些时候就要读图替代运算减少运算

4：AlphaCut会打断Early-z流程，因此AlphaCut有时候性能消耗比AlphaBlend高，不过实际需要看渲染AlphaCut减少了OverDraw的性能消耗，实际情况移动端并非AlphaCut固定比AlphaBlend消耗大，而在于Overdraw的减少消耗对比打断Early-z导致的额外开销。例如无论移动端还是PC端在大面积的草效果选用AlphaCut就会比使用Alphablend的性能更好，当然锯齿情况就要另外优化

###### IMR总结

1：前向渲染会产生更多overdraw,需要靠程序员自己剔除(例如软光栅)，延迟渲染在IMR架构更有优势

2：不会出现AlphaCut性能比AlphaBlend性能低奇怪的情况

3：IMR比TBR/TBDR更快更直接，不需要额外写入顶点缓存



