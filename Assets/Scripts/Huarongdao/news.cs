using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class news: MonoBehaviour
{
    private XRRayInteractor rightInteractor;
    public Transform rightController; // Pico 右手柄的 Transform
    public LayerMask chessPieceLayer; // 棋子方块的层
    public LayerMask chessBoardLayer; // 棋盘的层

    private GameObject draggableChessPiece; // 当前可拖拽的棋子
    private Vector3 dragStartPosition; // 拖拽起始位置
    private Vector3 dragEndPosition; // 拖拽终止位置
    private Vector3 moveFinalPosition;  // 移动终点
    private bool isDragging = false;
    private bool isMoving = false;
    private float moveSpeed = 1f;

    public InputActionReference triggerDown_Action;
    public InputActionReference triggerUp_Action;

    public UnityEngine.XR.InputDevice deviceLeft;
    public UnityEngine.XR.InputDevice deviceRight;

    //初始化移动参数
    void MoveChessPieceInit(GameObject chessPiece, Vector3 playerPosition, Vector3 oldPosition, Vector3 newPosition)
    {

        // 计算玩家位置到旧位置的向量
        Vector3 playerToOld = oldPosition - playerPosition;
        // 计算玩家位置到新位置的向量
        Vector3 playerToNew = newPosition - playerPosition;

        // 计算水平方向的夹角
        float angleHorizontal = Vector3.SignedAngle(Vector3.ProjectOnPlane(playerToOld, Vector3.up), Vector3.ProjectOnPlane(playerToNew, Vector3.up), Vector3.up);

        // 计算垂直方向的夹角
        float angleVertical = Vector3.SignedAngle(Vector3.ProjectOnPlane(playerToOld, Vector3.right), Vector3.ProjectOnPlane(playerToNew, Vector3.right), Vector3.right);

        /*
        // 计算水平方向的夹角
        float angleHorizontal = Vector3.SignedAngle(playerToOld, playerToNew, Vector3.up);
        // 计算垂直方向的夹角
        float angleVertical = Vector3.SignedAngle(playerToOld, playerToNew, Vector3.right);
        */
        // 判断水平方向夹角和垂直方向夹角的大小关系来确定相对位置

        Debug.Log("Mathf.Abs(angleHorizontal)=" + Mathf.Abs(angleHorizontal) + ", Mathf.Abs(angleVertical)=" + Mathf.Abs(angleVertical));

        if (Mathf.Abs(angleHorizontal) > Mathf.Abs(angleVertical))
        {
            if (angleHorizontal > 0)
            {
                moveFinalPosition = chessPiece.transform.position + Vector3.right; // 在玩家视角的右边
            }
            else
            {
                moveFinalPosition = chessPiece.transform.position + Vector3.left; // 在玩家视角的左边
            }
        }
        else
        {
            if (angleVertical > 0)
            {
                Debug.Log(chessPiece.name+"下移1格");
                moveFinalPosition = chessPiece.transform.position + Vector3.down; // 在玩家视角的下边
            }
            else
            {
                Debug.Log(chessPiece.name + "上移1格");
                moveFinalPosition = chessPiece.transform.position + Vector3.up; // 在玩家视角的上边
            }
        }

        isMoving = true;
    }

    void MoveUpdate(GameObject gameObject)
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, moveFinalPosition, moveSpeed * Time.deltaTime);
        

        if(Vector3.Distance(gameObject.transform.position, moveFinalPosition)<0.0001f)
        {
            isMoving = false;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        deviceLeft = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        deviceRight = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }



    // Update is called once per frame
    void Update()
    {

        bool isPressed =false;
        float value;
        if(deviceRight.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out value) && !value.Equals(0)) 
        {
            isPressed = true;
        }
       



        if (!isMoving && !isDragging && Physics.Raycast(rightController.position, rightController.forward, out RaycastHit hit, Mathf.Infinity, chessPieceLayer))
        {
            
            

            //按下trigger选中物体
            if (isPressed)
            {
                Debug.Log("选中"+hit.collider.name);
                isDragging = true;
                draggableChessPiece = hit.collider.gameObject;
                dragStartPosition = draggableChessPiece.transform.position;
            }
        }


        if(isDragging)
        {

            //释放trigger移动物体
            if(!isPressed)
            {
                Debug.Log("释放trigger移动物体");
                isDragging = false;

                if (Physics.Raycast(rightController.position, rightController.forward, out RaycastHit hit1, Mathf.Infinity, chessBoardLayer))
                {
                    dragEndPosition = hit1.point;
                    MoveChessPieceInit(draggableChessPiece, rightController.position, dragStartPosition, dragEndPosition);
                }
            }
        }

        if (isMoving)
        {
            MoveUpdate(draggableChessPiece);
        }
    }
}
