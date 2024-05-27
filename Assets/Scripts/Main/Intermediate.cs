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
 
    float timer = 0f;
    float waitTime = 3f; // 模拟加载需要的时间

    private void Start() {
        slider = GetComponent<Slider>();
        StartCoroutine("LoadScene");
    }
 
    IEnumerator LoadScene() {
        async = SceneManager.LoadSceneAsync(LoadScenesUtils.nextSceneName);
        async.allowSceneActivation = false;

        while (!async.isDone || timer < waitTime) {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / waitTime); // 计算加载进度
            slider.value = progress;
            if (progress >= 0.99f && async.progress >= 0.9f) {
                Debug.Log("异步场景加载完成，执行跳转"+LoadScenesUtils.nextSceneName);
                // 当加载完成或时间到达后，允许切换场景
                async.allowSceneActivation = true;
            }
            yield return null;
        }

    }
}
