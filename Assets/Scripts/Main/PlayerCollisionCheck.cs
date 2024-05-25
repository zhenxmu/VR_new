using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/**
 * 只需要放到玩家身上
 */
public class PlayerCollisionCheck : MonoBehaviour {
    // private bool rotController = true;
    private void Start() {
        Scene currentScene = SceneManager.GetActiveScene();
        
        // 是否是主场景初次启动
        if (LoadScenesUtils.isOriginalStart && currentScene.name == LoadScenesUtils.mainSceneName) {
            Debug.Log("主场景初次启动");
            LoadScenesUtils.isOriginalStart = false;
            return;
        }

        // 是否是主场景
        if (currentScene.name == LoadScenesUtils.mainSceneName) {
            Debug.Log("主场景玩家从保留位置恢复 "+ LoadScenesUtils.originalPositionMain+" "+LoadScenesUtils.originalRotationMain);
            transform.position = LoadScenesUtils.originalPositionMain;
            transform.rotation = Quaternion.Euler(LoadScenesUtils.originalRotationMain);
        }
        // // 标记该对象跨场景保留
        // DontDestroyOnLoad(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        // 防止回到主场景重复跳转
        if (LoadScenesUtils.isOnDoor) {
            Debug.Log("已经在门上，不执行跳转");
            return;
        }

        string objName = other.gameObject.name;
        if (!objName.StartsWith("DoorTo")) {
            Debug.Log(objName+" 非门对象，不执行跳转");
            return;
        }

        // 当前场景为主场景时，保存当前位置信息，玩家转向
        if (SceneManager.GetActiveScene().name == LoadScenesUtils.mainSceneName) {
            // 将物体沿着Y轴旋转180度
            // if (rotController) {
            transform.Rotate(0f, 180f, 0f);
            // }
            // rotController = !rotController;
            // Debug.Log("retController: "+rotController + " "+transform.rotation.eulerAngles);

            // 保存当前位置信息
            LoadScenesUtils.saveOriginalPosition(transform.position, transform.rotation.eulerAngles);
        }

        // 标记已经在门上, isOnDoor: false -> true
        LoadScenesUtils.isOnDoor = true;

        // 跳转到指定场景, isOnDoor: true -> true/false
        LoadScenesUtils.LoadSceneIfByObjNameOrElseNone(objName);
        Debug.Log("当前isOnDoor: "+LoadScenesUtils.isOnDoor);
    }

    private void OnTriggerExit(Collider other) {
        Debug.Log("离开门");
        LoadScenesUtils.isOnDoor = false;
        Debug.Log("当前isOnDoor: "+LoadScenesUtils.isOnDoor);
    }
}
