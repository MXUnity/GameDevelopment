using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Collections;

public class CustomDRP : RenderPipeline
{
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        
        for(int i =0; i < cameras.Length;i++)
        {
            ExecuteRenderLoop(cameras[i],CalculateCameraCulling(cameras[i]),context);
        }
        
        context.Submit();
    }

    //计算镜头裁剪信息
    public static CullingResults CalculateCameraCulling(Camera camera)
    {
        CullingResults cullingReuslts = new CullingResults();

        return cullingReuslts;
    }


    static void ExecuteRenderLoop(Camera camera,CullingResults cullResults,ScriptableRenderContext context)
    {
        var albedo = new AttachmentDescriptor(RenderTextureFormat.ARGB32);
        var specRough = new AttachmentDescriptor(RenderTextureFormat.ARGB32);
        var normal = new AttachmentDescriptor(RenderTextureFormat.ARGB2101010);
        var emission = new AttachmentDescriptor(RenderTextureFormat.ARGBHalf);
        var depth = new AttachmentDescriptor(RenderTextureFormat.Depth);

        emission.ConfigureClear(Color.clear,1.0f,0);
        depth.ConfigureClear(new Color(),1.0f,0);
        albedo.ConfigureTarget(BuiltinRenderTextureType.CameraTarget,false,true);
        var attachments = new NativeArray<AttachmentDescriptor>(5,Allocator.Temp);
        const int depthIndex = 0, albedoIndex = 1, specRoughIndex = 2, normalIndex = 3, emissionIndex = 4;
        attachments[depthIndex] = depth;
        attachments[albedoIndex] = albedo;
        attachments[specRoughIndex] = specRough;
        attachments[normalIndex] = normal;
        attachments[emissionIndex] = emission;
        context.BeginRenderPass(camera.pixelWidth, camera.pixelHeight, 1, attachments, depthIndex);
        attachments.Dispose();

        // Start the first subpass, GBuffer creation: render to albedo, specRough, normal and emission, no need to read any input attachments
        var gbufferColors = new NativeArray<int>(4, Allocator.Temp);
        gbufferColors[0] = albedoIndex;
        gbufferColors[1] = specRoughIndex;
        gbufferColors[2] = normalIndex;
        gbufferColors[3] = emissionIndex;
        context.BeginSubPass(gbufferColors);
        gbufferColors.Dispose();

        // Render the deferred G-Buffer
        // RenderGBuffer(cullResults, camera, context);
        
        context.EndSubPass();

        // Second subpass, lighting: Render to the emission buffer, read from albedo, specRough, normal and depth.
        // The last parameter indicates whether the depth buffer can be bound as read-only.
        // Note that some renderers (notably iOS Metal) won't allow reading from the depth buffer while it's bound as Z-buffer,
        // so those renderers should write the Z into an additional FP32 render target manually in the pixel shader and read from it instead
        var lightingColors = new NativeArray<int>(1, Allocator.Temp);
        lightingColors[0] = emissionIndex;
        var lightingInputs = new NativeArray<int>(4, Allocator.Temp);
        lightingInputs[0] = albedoIndex;
        lightingInputs[1] = specRoughIndex;
        lightingInputs[2] = normalIndex;
        lightingInputs[3] = depthIndex;
        context.BeginSubPass(lightingColors, lightingInputs, true);
        lightingColors.Dispose();
        lightingInputs.Dispose();

        // PushGlobalShadowParams(context);
        // RenderLighting(camera, cullResults, context);

        context.EndSubPass();

        // Third subpass, tonemapping: Render to albedo (which is bound to the camera target), read from emission.
        var tonemappingColors = new NativeArray<int>(1, Allocator.Temp);
        tonemappingColors[0] = albedoIndex;
        var tonemappingInputs = new NativeArray<int>(1, Allocator.Temp);
        tonemappingInputs[0] = emissionIndex;
        context.BeginSubPass(tonemappingColors, tonemappingInputs, true);
        tonemappingColors.Dispose();
        tonemappingInputs.Dispose();

        // present frame buffer.
        // FinalPass(context);

        context.EndSubPass();

        context.EndRenderPass();


    }
    
}
