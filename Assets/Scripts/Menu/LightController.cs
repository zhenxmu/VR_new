using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class LightController : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public VolumeProfile volumeProfile;
    void Start()
    {
        slider.onValueChanged.AddListener(OnBloomThresholdChanged);
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
        
    }
}
