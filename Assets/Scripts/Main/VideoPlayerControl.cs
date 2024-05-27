using DG.Tweening;
using UnityEngine.Video;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;



public class VideoPlayerControl : MonoBehaviour
{
    public VideoPlayer myVideoPlayer;
    private Vector3 videoOriginScale;
    private bool isplaying = false;
    // Start is called before the first frame update
    void Start()
    {
        videoOriginScale = myVideoPlayer.transform.localScale;
        myVideoPlayer.gameObject.SetActive(false);
        myVideoPlayer.Stop();
    }
    public void OnActivateEnterd()
    {
        Debug.Log("点击");
        if (!isplaying)
        {
            PlayVideo();

        }
        else
        {
            StopVideo();

        }
        isplaying = !isplaying;


    }
    private IEnumerator ToggleIsPlayingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isplaying = !isplaying;
    }
    public void OnHovereEnterd()
    {
        Debug.Log("看击");
        if (!isplaying)
        {
            PlayVideo();
            StartCoroutine(ToggleIsPlayingAfterDelay(1.0f)); // 延迟1秒后更改isplaying状态
        }



    }
    public void OnHovereExisted()
    {
        Debug.Log("离开看击");
        if (isplaying)
        {
            StopVideo();
            StartCoroutine(ToggleIsPlayingAfterDelay(1.0f)); // 延迟1秒后更改isplaying状态
        }



    }
    private void PlayVideo()
    {
        myVideoPlayer.gameObject.SetActive(true);
        myVideoPlayer.transform.localScale = new Vector3(0, videoOriginScale.y, videoOriginScale.z);
        myVideoPlayer.GetComponent<MeshRenderer>().material.color = Color.black;

        myVideoPlayer.transform.DOScale(videoOriginScale, 1.0f).SetEase(Ease.InQuint).onComplete += () => { myVideoPlayer.Play(); };
        myVideoPlayer.started += source => { myVideoPlayer.GetComponent<MeshRenderer>().material.DOColor(Color.white, 2f); };

    }
    private void StopVideo()
    {
        Tweener _tweenerColor = myVideoPlayer.GetComponent<MeshRenderer>().material.DOColor(Color.black, 1.0f);
        Tweener _tweenerScale = myVideoPlayer.transform.DOScale(0, 1.0f).SetEase(Ease.InQuint);
        _tweenerScale.onComplete += () => { myVideoPlayer.gameObject.SetActive(false); myVideoPlayer.Stop(); };
        Sequence _seq = DOTween.Sequence();
        _seq.Append(_tweenerColor);
        _seq.Append(_tweenerScale);

    }

}

