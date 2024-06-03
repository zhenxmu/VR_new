using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TestRaycastEvent : MonoBehaviour
{
    public Material normal;
    public Material raycast;


    /// <summary>
    /// 悬停进入
    /// </summary>
    /// <param name="args"></param>
    public void FirstHoverEntered(HoverEnterEventArgs args)
    {
        gameObject.GetComponent<MeshRenderer>().material = raycast;
    }

    /// <summary>
    /// 悬停退出
    /// </summary>
    /// <param name="args"></param>
    public void LastHoverExited(HoverExitEventArgs args)
    {
        gameObject.GetComponent<MeshRenderer>().material = normal;
    }

    /// <summary>
    /// 选择进入
    /// </summary>
    /// <param name="args"></param>
    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        gameObject.GetComponent<MeshRenderer>().material = raycast;
    }

    /// <summary>
    /// 选择退出
    /// </summary>
    /// <param name="args"></param>
    public void OnSelectExited(SelectExitEventArgs args)
    {
        gameObject.GetComponent<MeshRenderer>().material = normal;
    }
}