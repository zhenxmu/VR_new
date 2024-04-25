using UnityEngine;
using UnityEngine.Rendering;

public class SetRenderPipeline : MonoBehaviour
{
   public RenderPipelineAsset[] qualityLevelPipelines;

    void Start()
    {
        int currentQualityLevel = QualitySettings.GetQualityLevel();
        if (qualityLevelPipelines.Length > currentQualityLevel && qualityLevelPipelines[currentQualityLevel] != null)
        {
            GraphicsSettings.renderPipelineAsset = qualityLevelPipelines[currentQualityLevel];
        }
        else
        {
            Debug.LogError("No RenderPipelineAsset assigned for the current quality level or array out of index.");
        }
    }
}