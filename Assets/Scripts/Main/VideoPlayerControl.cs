using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;
using UnityEngine.Video;
using Unity.XR.CoreUtils;



public class VideoPlayerControl : MonoBehaviour
{
    public VideoPlayer myVideoPlayer;
    private Vector3 videoOriginScale;
    private bool isplaying=false;
    // Start is called before the first frame update
    void Start()
    {
        videoOriginScale=myVideoPlayer.transform.localScale;
        myVideoPlayer.gameObject.SetActive(false);
        myVideoPlayer.Stop();
    }
    public void OnActivateEnterd()
    {
        Debug.Log("点击");
        if(!isplaying)
        {
            PlayVideo();
            
        }else{
            StopVideo();

        }
        isplaying=!isplaying;


    }
    private void PlayVideo()
    {
        myVideoPlayer.gameObject.SetActive(true);
        myVideoPlayer.transform.localScale=new Vector3(0,videoOriginScale.y,videoOriginScale.z);
        myVideoPlayer.GetComponent<MeshRenderer>().material.color=Color.black;

        myVideoPlayer.transform.DOScale(videoOriginScale,1.0f).SetEase(Ease.InQuint).onComplete += () => {myVideoPlayer.Play();};
        myVideoPlayer.started += source => {myVideoPlayer.GetComponent<MeshRenderer>().material.DOColor(Color.white,2f);};

    }
    private void StopVideo()
    {
        Tweener _tweenerColor=myVideoPlayer.GetComponent<MeshRenderer>().material.DOColor(Color.black,1.0f);
        Tweener _tweenerScale=myVideoPlayer.transform.DOScale(0,1.0f).SetEase(Ease.InQuint);
        _tweenerScale.onComplete += () =>{myVideoPlayer.gameObject.SetActive(false);myVideoPlayer.Stop();};
        Sequence _seq=DOTween.Sequence();
        _seq.Append(_tweenerColor);
        _seq.Append(_tweenerScale);

    }
   
}
