

# Deferred Rendering

####  	概念

​	参考https://zhuanlan.zhihu.com/p/28489928

​	延迟渲染目的实际上是分离处理，是把渲染过程分成两部分来处理，

​	1.预先渲染出场景到屏幕上的数据 

​	2.对屏幕上的数据做着色运算处理

​	最大程度精简了对需要大量运算的片元运算部分

​	

#### 	相关概念片

###### 		MRT(multi-target Rendering)

​		参考：https://copyfuture.com/blogs-details/20201012184514587ioi81tjxzbes5ir

#### 	优化技巧

##### 		G-Buffer压缩

##### 		分块处理屏幕灯光

#### 	实现

##### 		UnityURP 实现 延迟渲染

​		关联基础相关内容：

​		参考https://zhuanlan.zhihu.com/p/401602488

#### 	使用建议