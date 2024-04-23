using UnityEngine;

public class Arrow_Control : MonoBehaviour
{
    // 所有的图片
    public Texture2D[] m_Images; // 直接在Unity编辑器中赋值
    // 切换图片的时间
    private float m_Time = 0;
    // 图片计数器
    private int m_CurrentIndex = 0;
    // 切换图片的间隔比例
    public float m_Fps = 25; // 可以在Unity编辑器中调整
    // 自身的Renderer组件
    private Renderer m_Renderer;

    void Start()
    {
        // 获取到自身的Renderer
        m_Renderer = gameObject.GetComponent<Renderer>();
    }

    void Update()
    {
        m_Time += Time.deltaTime;
        // 根据m_Fps计算间隔时间更换一次图片
        if (m_Time >= 1.0 / m_Fps)
        {
            m_CurrentIndex++;
            m_Time = 0;
        }
        // 计数器读取到最后一张图片之后，循环回第一张
        if (m_CurrentIndex >= m_Images.Length)
        {   
            m_CurrentIndex = 0;
        }
        // 直接从m_Images数组中获取Texture赋值给材质
        if (m_Images.Length > 0 && m_Images[m_CurrentIndex] != null) // 确保数组不为空且当前索引的Texture不为null
        {
            m_Renderer.material.mainTexture = m_Images[m_CurrentIndex];
        }
    }
}