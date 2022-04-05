using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Collections;

public class CustomDRP : RenderPipeline
{
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {

        
        context.Submit();
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

    }
    
}
