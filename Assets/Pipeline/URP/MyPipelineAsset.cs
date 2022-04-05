using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
#endif
    
public enum PipelineType
{
    Forward,
    ForwardUp,
    Deferred,
}

public class MyPipelineAsset : RenderPipelineAsset
{

    public PipelineType pipelineType = PipelineType.Deferred;
    protected override RenderPipeline CreatePipeline()
    {
        switch(pipelineType)
        {
            case PipelineType.Deferred:
                return new CustomDRP();
            

        }

        return new DefaultPipeline();
    }


    #region Editor
    #if UNITY_EDITOR
    [MenuItem("Assets/Create/Rendering/CreateMyPipeline")]
    public static void CreateMyPipeline()
    {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, CreateInstance<CreateMyPipelineAsset>(),
            "MyPipeline.asset", null, null);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812")]
    internal class CreateMyPipelineAsset : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            AssetDatabase.CreateAsset(CreateInstance<MyPipelineAsset>(), pathName);
        }
    }
    #endif
    #endregion
}
