using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScenesUtils : MonoBehaviour {
    // 是否是主场景初始启动
    public static bool isOriginalStart = true;

    // 是否在门上
    public static bool isOnDoor = false;

    // 保存主场景中玩家的位置信息
    public static Vector3 originalPositionMain = new Vector3(0, 0, 0);
    public static Vector3 originalRotationMain = new Vector3(0, 0, 0);
    public static string mainSceneName = "VRmain";

    // 根据碰撞物体名跳转到指定场景
    public static void LoadSceneIfByObjNameOrElseNone(string objName){
        string sceneName = objName.Substring(6);
        if (sceneName.StartsWith("Scene")) {
            SceneManager.LoadScene(sceneName);
            // 跳转到其他场景标记已经离开门, 因为其他场景的是自己摆的
            isOnDoor = false;
            Debug.Log("跳转到场景："+sceneName);
        } else if (sceneName.StartsWith("Main")) {
            SceneManager.LoadScene(mainSceneName);
            Debug.Log("跳转到主场景");
        } else {
            Debug.Log("未定义的场景跳转名");
        }
    }

    // 保存主场景中的位置信息到静态类
    public static void saveOriginalPosition(Vector3 position, Vector3 rotation) {
        Debug.Log("保存主场景位置信息 "+position+" "+rotation);
        originalPositionMain = position;
        originalRotationMain = rotation;
    }
}
