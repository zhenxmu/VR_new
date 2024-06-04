using UnityEngine;

public class MaterialPropertiesController : MonoBehaviour
{
    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<MeshRenderer>();
        if (objectRenderer == null)
        {
            Debug.LogError("Renderer component not found on the object!");
        }
    }

    public void SetMetallicToOne()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.SetFloat("_Metallic", 1.0f);
        }
    }

    public void SetMetallicToZero()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.SetFloat("_Metallic", 0.0f);
        }
    }
}