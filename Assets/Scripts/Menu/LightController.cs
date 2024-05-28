using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class LightController : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public VolumeProfile volumeProfile;
    private Light[] lightsInScene; // 声明一个Light数组
    void Start()
    {
        slider.value = 0.3f;
        slider.onValueChanged.AddListener(OnBloomThresholdChanged);
        lightsInScene = GameObject.FindGameObjectsWithTag("SceneLight").
                        Select(go => go.GetComponent<Light>()).
                        ToArray();
    }

    private void OnBloomThresholdChanged(float value)
    {
        if(volumeProfile.TryGet<Bloom>(out var bloom))
        {
            bloom.intensity.value = value * 9.0f;
        }
    }


    // Update is called once per frame
    void Update()
    {
        // 在Update()函数中检测Slider的值的变化
        float newIntensity = slider.value;

        // 遍历所有"LightScene"标签的Light物体,并修改它们的"发射"的"强度"值
        foreach (Light light in lightsInScene)
        {
            light.intensity = newIntensity;
        }
    }
}
