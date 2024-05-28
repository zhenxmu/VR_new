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
    private Light[] lightsInScene; // ����һ��Light����
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
        // ��Update()�����м��Slider��ֵ�ı仯
        float newIntensity = slider.value;

        // ��������"LightScene"��ǩ��Light����,���޸����ǵ�"����"��"ǿ��"ֵ
        foreach (Light light in lightsInScene)
        {
            light.intensity = newIntensity;
        }
    }
}
