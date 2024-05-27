using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class LightController : MonoBehaviour
{
    public Slider slider;
    public VolumeProfile volumeProfile;
    // Start is called before the first frame update
    void Start()
    {
        slider.onValueChanged.AddListener(OnBloomThresholdChanged);
    }

    private void OnBloomThresholdChanged(float value)
    {
        // 获取 Volume Profile 中的 Bloom 设置
        if (volumeProfile.TryGet<Bloom>(out var bloom))
        {
            // 更新 Bloom 的阈值参数
            bloom.intensity.value = value * 9.0f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
