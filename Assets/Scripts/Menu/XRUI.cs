using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRUI : MonoBehaviour
{
    public Transform cameraTransform;
    [Header("�ӳ�")]
    public float delay = 2.0f; // �ӳ�ʱ��
    [Header("����ƶ��ٶ�")]
    public float maxMoveSpeed = 10.0f; // ����ƶ��ٶ�  
    [Header("ƽ��ʱ��")]
    public float smoothTime = 0.5f; // ƽ��ʱ�䣬���ڼ����ֵ�ٶ�  
    [Header("������ֵ")]
    public float distanceThreshold = 0.01f; // �����жϽӽ�����ľ�����ֵ 
    [Header("����")]
    public float targetDistance; // Ŀ�����  

    private Vector3 velocity = Vector3.zero; // �ٶ�����ƽ���ƶ�  
    private float currentDistance; // ��ǰ����
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
        // ����ģ�͵�Ŀ��λ��  
        targetPositions = cameraTransform.transform.position + cameraTransform.transform.forward * targetDistance;

        if (Vector3.Distance(transform.position, targetPositions) < distanceThreshold)
        {
            isT = true;
            yield return null;
        }
        else
        {
            // �ȴ�ָ�����ӳ�ʱ��  
            yield return new WaitForSeconds(delay);


            while (!isT)
            {
                // ���㵱ǰ�����Ŀ�����  
                currentDistance = Vector3.Distance(transform.position, cameraTransform.position);

                // ���ݾ�������ƶ��ٶȣ�����ʹ��һ���򵥵ķ�������ϵ  
                // ����Ը�����Ҫ����������㷽ʽ  
                float moveSpeed = maxMoveSpeed / (currentDistance + 0.1f); // ��������㣬�����һ��Сֵ��ƽ������  

                // ����ƽ���ƶ���Ŀ��λ��  
                Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetPositions, ref velocity, smoothTime);

                // ���㵱ǰ��ת�Ƕ�
                targetRotations = Quaternion.LookRotation(transform.position - cameraTransform.transform.position);
                // �����ٶȺ�ʱ��������ģ�͵�λ��  
                transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime), Quaternion.Slerp(transform.rotation, targetRotations, moveSpeed * Time.deltaTime));
                if (Vector3.Distance(transform.position, targetPositions) < distanceThreshold)
                {
                    // ����㹻�ӽ�����ֹͣ�ƶ�  
                    isT = true;
                    Debug.Log("ģ���Ѿ��ӽ����λ��");
                    break;
                }

                // �ȴ���һ֡  
                yield return null;
            }
        }
    }

}