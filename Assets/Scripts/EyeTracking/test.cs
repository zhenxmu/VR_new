using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;

public class test : MonoBehaviour
{
    
    TrackingStateCode trackingState;
     
    // Start is called before the first frame update
    void Start()
    {
        trackingState = (TrackingStateCode)PXR_MotionTracking.WantEyeTrackingService();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void getEyeinfo()
    {
        EyeTrackingMode eyeTrackingMode = EyeTrackingMode.PXR_ETM_NONE;
        bool supported = false;
       int supportedModesCount = 0;
       trackingState = (TrackingStateCode)PXR_MotionTracking.GetEyeTrackingSupported(ref supported,ref supportedModesCount, ref eyeTrackingMode);
       Debug.Log("is"+trackingState);

    }
}
