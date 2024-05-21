using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRUI : MonoBehaviour
{
    public Transform cameraTransform;
    [Header("延迟")]
    public float delay = 2.0f; // 延迟时间
    [Header("最大移动速度")]
    public float maxMoveSpeed = 10.0f; // 最大移动速度  
    [Header("平滑时间")]
    public float smoothTime = 0.5f; // 平滑时间，用于计算插值速度  
    [Header("距离阈值")]
    public float distanceThreshold = 0.01f; // 用于判断接近相机的距离阈值 
    [Header("距离")]
    public float targetDistance; // 目标距离  

    private Vector3 velocity = Vector3.zero; // 速度用于平滑移动  
    private float currentDistance; // 当前距离
    private Vector3 targetPositions;
    private Quaternion targetRotations;

    private bool isT = true;

    void Update()
    {
        if (isT)
        {
            isT = false;
            StartCoroutine(FollowCameraWithDelay());
        }
    }

    IEnumerator FollowCameraWithDelay()
    {
        // 计算模型的目标位置  
        targetPositions = cameraTransform.transform.position + cameraTransform.transform.forward * targetDistance;

        if (Vector3.Distance(transform.position, targetPositions) < distanceThreshold)
        {
            isT = true;
            yield return null;
        }
        else
        {
            // 等待指定的延迟时间  
            yield return new WaitForSeconds(delay);


            while (!isT)
            {
                // 计算当前距离和目标距离  
                currentDistance = Vector3.Distance(transform.position, cameraTransform.position);

                // 根据距离计算移动速度，这里使用一个简单的反比例关系  
                // 你可以根据需要调整这个计算方式  
                float moveSpeed = maxMoveSpeed / (currentDistance + 0.1f); // 避免除以零，并添加一个小值以平滑过渡  

                // 计算平滑移动的目标位置  
                Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetPositions, ref velocity, smoothTime);

                // 计算当前旋转角度
                targetRotations = Quaternion.LookRotation(transform.position - cameraTransform.transform.position);
                // 根据速度和时间来更新模型的位置  
                transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime), Quaternion.Slerp(transform.rotation, targetRotations, moveSpeed * Time.deltaTime));
                if (Vector3.Distance(transform.position, targetPositions) < distanceThreshold)
                {
                    // 如果足够接近，则停止移动  
                    isT = true;
                    Debug.Log("模型已经接近相机位置");
                    break;
                }

                // 等待下一帧  
                yield return null;
            }
        }
    }

}