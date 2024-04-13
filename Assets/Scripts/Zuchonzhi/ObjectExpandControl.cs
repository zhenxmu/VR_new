using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;
using UnityEngine.Video;
using Unity.XR.CoreUtils;
using NodeCanvas.Framework;

public class ObjectExpandControl : MonoBehaviour
{
    public GameObject objectToExpand; // Changed from VideoPlayer to GameObject
    private Vector3 objectOriginScale;
    private bool isExpanded = false; // Renamed variable for clarity
    

    // Start is called before the first frame update
    void Start()
    {
        objectOriginScale = objectToExpand.transform.localScale;
        objectToExpand.gameObject.SetActive(false);
    
    }

    public void OnActivateEntered() // Corrected typo in method name
    {
        Debug.Log("Activated");
        if (!isExpanded)
        {
            ExpandObject();
        }
        else
        {
            CollapseObject();
        }
        isExpanded = !isExpanded;
    }

    private void ExpandObject()
    {
        objectToExpand.gameObject.SetActive(true);
        objectToExpand.transform.localScale = new Vector3(0, objectOriginScale.y, objectOriginScale.z);
        objectToExpand.GetComponent<MeshRenderer>().material.color = Color.black;

        // Animate scale to original size and change color to white
        objectToExpand.transform.DOScale(objectOriginScale, 1.0f).SetEase(Ease.InQuint);
        objectToExpand.GetComponent<MeshRenderer>().material.DOColor(Color.white, 2f);
    }

    private void CollapseObject()
    {
        Tweener tweenerColor = objectToExpand.GetComponent<MeshRenderer>().material.DOColor(Color.black, 1.0f);
        Tweener tweenerScale = objectToExpand.transform.DOScale(0, 1.0f).SetEase(Ease.InQuint);
        tweenerScale.onComplete += () => { objectToExpand.gameObject.SetActive(false); };
        Sequence seq = DOTween.Sequence();
        seq.Append(tweenerColor);
        seq.Append(tweenerScale);
    }
}