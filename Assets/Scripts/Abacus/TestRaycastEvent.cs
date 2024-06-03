using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TestRaycastEvent : MonoBehaviour
{
    public Material normal;
    public Material raycast;


    /// <summary>
    /// ��ͣ����
    /// </summary>
    /// <param name="args"></param>
    public void FirstHoverEntered(HoverEnterEventArgs args)
    {
        gameObject.GetComponent<MeshRenderer>().material = raycast;
    }

    /// <summary>
    /// ��ͣ�˳�
    /// </summary>
    /// <param name="args"></param>
    public void LastHoverExited(HoverExitEventArgs args)
    {
        gameObject.GetComponent<MeshRenderer>().material = normal;
    }

    /// <summary>
    /// ѡ�����
    /// </summary>
    /// <param name="args"></param>
    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        gameObject.GetComponent<MeshRenderer>().material = raycast;
    }

    /// <summary>
    /// ѡ���˳�
    /// </summary>
    /// <param name="args"></param>
    public void OnSelectExited(SelectExitEventArgs args)
    {
        gameObject.GetComponent<MeshRenderer>().material = normal;
    }
}