using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;
using System.Text.RegularExpressions;
using UnityEngine.Rendering;
using TMPro;

public class AbacusController : MonoBehaviour
{
    public TextMeshProUGUI dashBoard;//显示当前算盘所示数字
    private int[][] fiveAbacusArray;//01表示该算珠是否纳入计算
    private int[][] oneAbacusArray;
    private int sum;

    public Transform rightController; // Pico 右手柄的 Transform
    public LayerMask abacusLayer; // 算珠的层
    public LayerMask backgroundLayer; // 透明背景板的层

    private GameObject draggableAbacus; // 当前选中可拖拽的算珠
    private int draggableAbacus_Type; // 当前选中可拖拽的算珠类型（1 5）
    private int draggableAbacus_x, draggableAbacus_y; // 当前选中可拖拽的算珠命名中的x、y

    private Vector3 dragStartPosition; // 拖拽起始位置
    private Vector3 dragEndPosition; // 拖拽终止位置
    private Vector3 moveFinalPosition;  // 移动终点
    private bool isDragging = false;
    private bool isMoving = false;
    private bool isPressed;
    private float moveSpeed = 1.3f;

    private string fivePattern = @"^Five_\d+_\d+$";
    private string onePattern = @"^One_\d+_\d+$";

    public InputActionReference triggerDown_Action;
    public InputActionReference triggerUp_Action;

    public UnityEngine.XR.InputDevice deviceRight;

    void AbacusInit()
    {
        //创建二维数组  
        fiveAbacusArray = new int[9][];
        oneAbacusArray = new int[9][];
        sum = 0;
    }

    void XRDeviceinit()
    {
        deviceRight = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    void draggableAbacusInit(Collider hitCollider)
    {
        Debug.Log("选中" + hitCollider.name);
        isDragging = true;
        draggableAbacus = hitCollider.gameObject;
        dragStartPosition = draggableAbacus.transform.position;
    }

    //返回1表示允许向上移，返回-1表示允许向下移，返回0表示不允许移动
    int AbacusMoveAllowed(GameObject abacus)
    {
        // 对5值算珠
        if (draggableAbacus_Type == 5)
        {
            if (fiveAbacusArray[draggableAbacus_x][draggableAbacus_y] == 0)
            {
                if (draggableAbacus_y == 1 || fiveAbacusArray[draggableAbacus_x][draggableAbacus_y - 1] == 1)
                {
                    /*
                    fiveAbacusArray[draggableAbacus_x][draggableAbacus_y] = 1;
                    sum += (int)(Math.Pow(10, draggableAbacus_x) * 5);
                    */
                    return -1;
                }
                return 0;
            }
            if (fiveAbacusArray[draggableAbacus_x][draggableAbacus_y] == 1)
            {
                if (draggableAbacus_y == 2 || fiveAbacusArray[draggableAbacus_x][draggableAbacus_y + 1] == 0)
                {
                    return 1;
                }
                return 0;
            }
        }

        //对1值算珠
        if (draggableAbacus_Type == 1)
        {
            if (oneAbacusArray[draggableAbacus_x][draggableAbacus_y] == 0)
            {
                if (draggableAbacus_y == 1 || oneAbacusArray[draggableAbacus_x][draggableAbacus_y - 1] == 1)
                {
                    return 1;
                }
                return 0;
            }
            if (oneAbacusArray[draggableAbacus_x][draggableAbacus_y] == 1)
            {
                if (draggableAbacus_y == 5 || oneAbacusArray[draggableAbacus_x][draggableAbacus_y + 1] == 0)
                {
                    return -1;
                }
                return 0;
            }
        }

        Debug.Log("draggableAbacus_Type不为1或5，出错！draggableAbacus_Type="+draggableAbacus_Type);
        return 0;
    }

    void MoveAbacusInit(GameObject abacus, Vector3 playerPosition, Vector3 oldPosition, Vector3 newPosition)
    {
        int abacusMoveAllowed = AbacusMoveAllowed(abacus);
        // 若该算珠不能合法移动，直接返回
        if (abacusMoveAllowed == 0)
        {
            return;
        }
        // 计算位移变量
        float deltaYPosition = abacusMoveAllowed;
        if (draggableAbacus_Type == 1)
        {
            deltaYPosition *= 2.3f;
        }
        if (draggableAbacus_Type == 5)
        {
            deltaYPosition *= 1.3f;
        }
        // 计算玩家位置到旧位置的向量
        Vector3 playerToOld = oldPosition - playerPosition;
        // 计算玩家位置到新位置的向量
        Vector3 playerToNew = newPosition - playerPosition;

        // 计算垂直方向的夹角
        float angleVertical = Vector3.SignedAngle(Vector3.ProjectOnPlane(playerToOld, Vector3.right), Vector3.ProjectOnPlane(playerToNew, Vector3.right), Vector3.right);

        if (angleVertical > 0 && abacusMoveAllowed == -1)// 在玩家视角的下边
        {
            //改变表示值
            if (draggableAbacus_Type == 5)
            {
                fiveAbacusArray[draggableAbacus_x][draggableAbacus_y] = 1;
                sum += (int)(Math.Pow(10, draggableAbacus_x) * 5);
                Debug.Log("当前算盘表示值为："+ sum);
            }

            if (draggableAbacus_Type == 1)
            {
                oneAbacusArray[draggableAbacus_x][draggableAbacus_y] = 0;
                sum -= (int)(Math.Pow(10, draggableAbacus_x) * 1);
                Debug.Log("当前算盘表示值为：" + sum);
            }

            Debug.Log(abacus.name + "下移1格");
            moveFinalPosition = abacus.transform.position;
            moveFinalPosition.y += deltaYPosition; 
            isMoving = true;
        }
        if(angleVertical < 0 && abacusMoveAllowed == 1)// 在玩家视角的上边
        {
            //改变表示值
            if (draggableAbacus_Type == 5)
            {
                fiveAbacusArray[draggableAbacus_x][draggableAbacus_y] = 0;
                sum -= (int)(Math.Pow(10, draggableAbacus_x) * 5);
                Debug.Log("当前算盘表示值为：" + sum);
            }

            if (draggableAbacus_Type == 1)
            {
                oneAbacusArray[draggableAbacus_x][draggableAbacus_y] = 1;
                sum += (int)(Math.Pow(10, draggableAbacus_x) * 1);
                Debug.Log("当前算盘表示值为：" + sum);
            }

            Debug.Log(abacus.name + "上移1格");
            moveFinalPosition = abacus.transform.position;
            moveFinalPosition.y += deltaYPosition; 
            isMoving = true;
        }
    }

    void MoveUpdate(GameObject gameObject)
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, moveFinalPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(gameObject.transform.position, moveFinalPosition) < 0.0001f)
        {
            isMoving = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        AbacusInit();
        XRDeviceinit();
    }

    // Update is called once per frame
    void Update()
    {
        isPressed = false;
        float value;
        if (deviceRight.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out value) && !value.Equals(0))
        {
            isPressed = true;
        }

        if (!isMoving && !isDragging && Physics.Raycast(rightController.position, rightController.forward, out RaycastHit hit, Mathf.Infinity, abacusLayer))
        {
            //若按下trigger选中的为5值算珠物体
            Match match = Regex.Match(hit.collider.name, fivePattern);
            if (isPressed && match.Success)
            {
                draggableAbacus_Type = 5;
                // 提取匹配的数字部分
                string xStr = match.Groups[1].Value;
                string yStr = match.Groups[2].Value;
                // 将提取的字符串转换为整数
                int.TryParse(xStr, out draggableAbacus_x);
                int.TryParse(yStr, out draggableAbacus_y);

                draggableAbacusInit(hit.collider);
            }

            //若按下trigger选中的为1值算珠物体
            match = Regex.Match(hit.collider.name, onePattern);
            if (isPressed && Regex.IsMatch(hit.collider.name, onePattern))
            {
                draggableAbacus_Type = 1;
                // 提取匹配的数字部分
                string xStr = match.Groups[1].Value;
                string yStr = match.Groups[2].Value;
                // 将提取的字符串转换为整数
                int.TryParse(xStr, out draggableAbacus_x);
                int.TryParse(yStr, out draggableAbacus_y);

                draggableAbacusInit(hit.collider);
            }
        }

        if (isDragging)
        {
            //释放trigger移动物体
            if (!isPressed)
            {
                Debug.Log("释放trigger移动物体");
                isDragging = false;

                if (Physics.Raycast(rightController.position, rightController.forward, out RaycastHit hit1, Mathf.Infinity, abacusLayer))
                {
                    dragEndPosition = hit1.point;
                    MoveAbacusInit(draggableAbacus, rightController.position, dragStartPosition, dragEndPosition);
                }
            }
        }

        if (isMoving)
        {
            dashBoard.text = sum.ToString();
            MoveUpdate(draggableAbacus);
        }
    }
}
