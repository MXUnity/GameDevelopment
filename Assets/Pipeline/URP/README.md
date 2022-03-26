

# Unity URP

#### URP入口

1：URP自定义管线可以通过资源形式存储，在游戏运行渲染时候调用RenderPipelineAsset.CreatePipeline获取管线对象。

继承RenderPipelineAsset的对象作为配置存储，在生命周期上进行回调创建RenderPipeline对象

2：RenderPipeline创建每次渲染后都会释放，不可重复使用，每次渲染时会调用Render接口



下面了解下URP里面核心渲染部分对象以及功能：

1.ScriptableRenderContext对象

Submit 函数：提交Cpu渲染设置，执行渲染结果



EmitWorldGeometryForSceneView

