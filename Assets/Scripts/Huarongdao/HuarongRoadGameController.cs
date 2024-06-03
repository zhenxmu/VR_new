using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HuarongRoadGameController : MonoBehaviour
{
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

    private GameObject hitObject;
    private Color originalColor;

    private int[,] chessBoardArray = new int[5, 4] { { 1, 0, 0, 1 }, { 1, 1, 1, 1 }, { 1, 1, 1, 1 }, { 1, 1, 1, 1 }, { 1, 1, 1, 1 } };

    //0-3：卒1-4，4：马超，5：张飞，6：黄忠，7：赵云，8：关羽，9：曹操
    //行
    private int[] chessXLocation = new int[10] { 0, 1, 1, 0, 2, 2, 4, 4, 3, 5 };
    //列
    private int[] chessYLocation = new int[10] { 0, 1, 2, 3, 0, 3, 0, 3, 1, 1 };
    //棋子高度
    static private int[] chessXSize = new int[10] { 1, 1, 1, 1, 2, 2, 2, 2, 1, 2 };
    //棋子宽度
    static private int[] chessYSize = new int[10] { 1, 1, 1, 1, 1, 1, 1, 1, 2, 2 };

    public static int GetChessNumber(string pieceName)
    {
        switch (pieceName)
        {
            case "Soldier1":
                return 0;
            case "Soldier2":
                return 1;
            case "Soldier3":
                return 2;
            case "Soldier4":
                return 3;
            case "Machao":
                return 4;
            case "Zhangfei":
                return 5;
            case "Huangzhong":
                return 6;
            case "Zhaoyun":
                return 7;
            case "Guanyu":
                return 8;
            case "Caocao":
                return 9;
            default:
                return -1; // 如果输入的棋子名字不在列表中，则返回 -1 表示无效输入
        }
    }

    bool MoveAllow(GameObject chessPiece, int xDir, int yDir)
    {
        int chessNum = GetChessNumber(chessPiece.name);

        //上/下移动
        if (xDir != 0)
        {
            int delta = 0;
            if (xDir < 0 && chessXSize[chessNum] == 2) 
                delta = -1;

            for (int i = 0; i < chessYSize[chessNum]; i++)
            {
                if (chessBoardArray[chessXLocation[chessNum] + delta + xDir, chessYLocation[chessNum] + i] != 0
                    || (chessXLocation[chessNum] + delta + xDir) < 0 || (chessXLocation[chessNum] + delta + xDir) > 4) 
                {
                    return false;
                }
            }

            //修改棋盘状态
            for (int i = 0; i < chessYSize[chessNum]; i++)
            {
                chessBoardArray[chessXLocation[chessNum] + delta + xDir, chessYLocation[chessNum] + i] = 1;
                chessBoardArray[chessXLocation[chessNum] + delta + xDir - xDir * chessXSize[chessNum], chessYLocation[chessNum] + i] = 0;
            }

            //修改棋子位置
            chessXLocation[chessNum] += xDir;

            return true;
        }

        //左/右移动
        else
        {
            int delta = 0;
            if (yDir < 0 && chessYSize[chessNum] == 2)
                delta = -1;
            for (int i = 0; i < chessXSize[chessNum]; i++)
            {
                if (chessBoardArray[chessXLocation[chessNum] + i, chessYLocation[chessNum] + delta + yDir] != 0
                    || (chessYLocation[chessNum] + delta + yDir) < 0 || (chessYLocation[chessNum] + delta + yDir) > 3)
                {
                    return false;
                }
            }

            //修改棋盘状态
            for (int i = 0; i < chessXSize[chessNum]; i++)
            {
                chessBoardArray[chessXLocation[chessNum] + i, chessYLocation[chessNum] + delta + yDir] = 1;
                chessBoardArray[chessXLocation[chessNum] + i, chessYLocation[chessNum] + delta + yDir - yDir * chessYSize[chessNum]] = 0;
            }

            //修改棋子位置
            chessYLocation[chessNum] += yDir;

            return true;
        }
        
    }

    //初始化移动参数
    void MoveChessPieceInit(GameObject chessPiece, Vector3 playerPosition, Vector3 oldPosition, Vector3 newPosition)
    {
        
        // 计算玩家位置到旧位置的向量
        Vector3 playerToOld = oldPosition - playerPosition;
        // 计算玩家位置到新位置的向量
        Vector3 playerToNew = newPosition - playerPosition;

        Vector3 myLeft = new Vector3(0.61232768f, 0f, -0.79060408f);
        Vector3 myRight = new Vector3(-0.61232768f, 0f, 0.79060408f);

        // 计算水平方向的夹角
        float angleHorizontal = Vector3.SignedAngle(Vector3.ProjectOnPlane(playerToOld, Vector3.up), Vector3.ProjectOnPlane(playerToNew, Vector3.up), Vector3.up);

        // 计算垂直方向的夹角
        float angleVertical = Vector3.SignedAngle(Vector3.ProjectOnPlane(playerToOld, myRight), Vector3.ProjectOnPlane(playerToNew, myRight), myRight);

        // 当前游戏物体的缩放值
        float scale = 0.2f;
        // 当前游戏物体的旋转值
        // Quaternion rotation = Quaternion.Euler(0, 52.242f, 0);
        // 计算向左移动的位移向量
        // Vector3 displacement = rotation * Vector3.left * 1f * scale;

        // 判断水平方向夹角和垂直方向夹角的大小关系来确定相对位置
        Debug.Log("Mathf.Abs(angleHorizontal)=" + Mathf.Abs(angleHorizontal) + ", Mathf.Abs(angleVertical)=" + Mathf.Abs(angleVertical));

        if (Mathf.Abs(angleHorizontal) > Mathf.Abs(angleVertical))
        {
            if (angleHorizontal > 0)
            {
                if(MoveAllow(chessPiece, 0, 1))
                {
                    moveFinalPosition = chessPiece.transform.position + myRight * scale; // 在玩家视角的右边
                    isMoving = true;
                }

            }
            else
            {
                if (MoveAllow(chessPiece, 0, -1))
                {
                    moveFinalPosition = chessPiece.transform.position + myLeft * scale; // 在玩家视角的左边
                    isMoving = true;
                }
                    
            }
        }
        else
        {
            if (angleVertical > 0)
            {
                Debug.Log(chessPiece.name+"下移1格");
                if (MoveAllow(chessPiece, -1, 0))
                {
                    moveFinalPosition = chessPiece.transform.position + Vector3.down * scale; // 在玩家视角的下边
                    isMoving = true;
                }
                    
            }
            else
            {
                Debug.Log(chessPiece.name + "上移1格");
                if (MoveAllow(chessPiece, 1, 0))
                {
                    moveFinalPosition = chessPiece.transform.position + Vector3.up * scale; // 在玩家视角的上边
                    isMoving = true;
                }   
            }
        }

        
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
            Debug.Log("trigger is pressed.");
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
