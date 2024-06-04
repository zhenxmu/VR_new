using UnityEngine;
using UnityEngine.Rendering;

public class SetRenderPipeline : MonoBehaviour
{
    public RenderPipelineAsset pipelineAsset; // 单个渲染管线资产

    void Start()
    {
        if (pipelineAsset != null)
        {

            // 设置当前质量级别的渲染管线
            QualitySettings.renderPipeline = pipelineAsset;

            Debug.Log("Applied Render Pipeline: " + pipelineAsset.name);
        }
        else
        {
            Debug.LogError("No RenderPipelineAsset assigned.");
        }
    }
}