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
        // ��ȡ Volume Profile �е� Bloom ����
        if (volumeProfile.TryGet<Bloom>(out var bloom))
        {
            // ���� Bloom ����ֵ����
            bloom.intensity.value = value * 9.0f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
