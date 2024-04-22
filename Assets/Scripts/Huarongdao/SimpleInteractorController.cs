using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SimpleInteractorController : MonoBehaviour
{
    public GameObject chessObject;
    public Material white, blue;

    public void OnSelected(XRBaseInteractor interactor)
    {
        
    }

    public void OnDeSelected(XRBaseInteractor interactor)
    {

    }

    public void OnHoverEnter(XRBaseInteractor interactor)
    {
        chessObject.GetComponent<MeshRenderer>().material = blue;
    }

    public void OnHoverExit(XRBaseInteractor interactor)
    {
        chessObject.GetComponent<MeshRenderer>().material = white;
    }

    public void OnActivated(XRBaseInteractor interactor)
    {

    }

    public void OnDeActivated(XRBaseInteractor interactor)
    {

    }

}
