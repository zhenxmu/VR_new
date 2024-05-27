using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * 挂载此脚本的游戏物体不被销毁，建议只将此脚本挂载到主场景的物体上
 * A -> B, B -> A 保证只有一个对象
 */

public class DontDestroy : MonoBehaviour {
    // 单例池记录所有跨场景对象的单例 objName : Object
    private static Dictionary<string, Object> objNameMap = new Dictionary<string, Object>();

    // 跨场景物体在不同场景中的位置(不需要指定主场景)
    /* 
        objName : {
            sceneName : position
        }
     */
    private static Dictionary<string, Dictionary<string, Vector3>> objNameToPosMap = new Dictionary<string, Dictionary<string, Vector3>>() {
        { "Capsule", new Dictionary<string, Vector3>() { { "SceneI", new Vector3(2, 2, 2) }, { "SceneII", new Vector3(4,4,4) } } },
        { "Cylinder", new Dictionary<string, Vector3>() { { "SceneI", new Vector3(4, 4, 4) }, { "SceneII", new Vector3(8, 8, 8) } } },
    };
    // 该物体在不同场景中的旋转
    private static Dictionary<string, Dictionary<string, Vector3>> objNameToRotMap = new Dictionary<string, Dictionary<string, Vector3>>() {
        { "Capsule", new Dictionary<string, Vector3>() { { "SceneI", new Vector3(45, 0, 0) }, { "SceneII", new Vector3(0, 0, 0) } } },
        { "Cylinder", new Dictionary<string, Vector3>() { { "SceneI", new Vector3(0, 0, 0) }, { "SceneII", new Vector3(0, 0, 0) } } },
    };

    // 指定当前对象存活场景
    public List<string> keepInSceneNames = new List<string>() { LoadScenesUtils.mainSceneName, };

    // 该物体在不同场景中的位置
    private Dictionary<string, Vector3> posMap;
    // 该物体在不同场景中的旋转
    private Dictionary<string, Vector3> rotMap;

    // 记录上一场景，性能优化，不需要每帧都检查
    private string lastSceneName = "";
    void Start() {
        // 单例池中已有对象不创建新对象
        if (objNameMap.ContainsKey(gameObject.name)) {
            // 整个对象都销毁掉了，这个脚本没有任何意义
            Destroy(gameObject);
            return;
        }
        // 新建对象保存到单例池
        objNameMap.Add(gameObject.name, gameObject);
        DontDestroyOnLoad(gameObject);
        // 记录上一个场景
        lastSceneName = SceneManager.GetActiveScene().name;
        // 保存主场景的位置信息
        if (lastSceneName == LoadScenesUtils.mainSceneName && !objNameToPosMap[gameObject.name].ContainsKey(LoadScenesUtils.mainSceneName)) {
            objNameToPosMap[gameObject.name][LoadScenesUtils.mainSceneName] = gameObject.transform.position;
            objNameToRotMap[gameObject.name][LoadScenesUtils.mainSceneName] = gameObject.transform.eulerAngles;
        }
        // 初始化位置和旋转
        posMap = objNameToPosMap.ContainsKey(gameObject.name) ? objNameToPosMap[gameObject.name] : new Dictionary<string, Vector3>();
        rotMap = objNameToRotMap.ContainsKey(gameObject.name) ? objNameToRotMap[gameObject.name] : new Dictionary<string, Vector3>();
    }


    // 每帧检查当前场景是否需要该物体, 因为主场景->其他场景时保留的物体不会执行start方法
    void Update() {
        string currentSceneName = SceneManager.GetActiveScene().name;
        // 当前场景不需要该物体（获得此物体需要转到主场景中重新创建），从单例池中移除
        if (!keepInSceneNames.Contains(currentSceneName) && objNameMap.ContainsKey(gameObject.name)) {
            objNameMap.Remove(gameObject.name);
            Destroy(gameObject);
        }
        // 如果当前场景不是主场景，就需要根据map里面的信息设置对象的位置和旋转
        if (currentSceneName != lastSceneName) {
            if (posMap.ContainsKey(currentSceneName)) {
                gameObject.transform.position = posMap[currentSceneName];
            }
            if (rotMap.ContainsKey(currentSceneName)) {
                gameObject.transform.eulerAngles = rotMap[currentSceneName];
            }
        }
        lastSceneName = currentSceneName;
    }
}