using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// 挂载在Slider上面
public class Intermediate : MonoBehaviour {
    // //显示进度的文本
    // private Text progress;
    //进度条的数值
    private float progressValue;
    private Slider slider;
    private AsyncOperation async = null;
 
    private void Start() {
        slider = GetComponent<Slider>();
        StartCoroutine("LoadScene");
    }
 
    IEnumerator LoadScene() {
        // 异步加载下一个
        async = SceneManager.LoadSceneAsync(LoadScenesUtils.nextSceneName);
        // 不允许显示
        async.allowSceneActivation = false;
        while (!async.isDone) {
            if (async.progress < 0.9f)
                progressValue = async.progress;
            else
                progressValue = 1.0f;
 
            slider.value = progressValue;
            // progress.text = (int)(slider.value * 100) + " %";
 
            if (progressValue >= 0.9) {
                Debug.Log("异步场景加载完成，执行跳转");
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
