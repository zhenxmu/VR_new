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
    public Transform rightController; // Pico ���ֱ��� Transform
    public LayerMask chessPieceLayer; // ���ӷ���Ĳ�
    public LayerMask chessBoardLayer; // ���̵Ĳ�

    private GameObject draggableChessPiece; // ��ǰ����ק������
    private Vector3 dragStartPosition; // ��ק��ʼλ��
    private Vector3 dragEndPosition; // ��ק��ֹλ��
    private Vector3 moveFinalPosition;  // �ƶ��յ�
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

    //0-3����1-4��4������5���ŷɣ�6�����ң�7�����ƣ�8������9���ܲ�
    //��
    private int[] chessXLocation = new int[10] { 0, 1, 1, 0, 2, 2, 4, 4, 3, 5 };
    //��
    private int[] chessYLocation = new int[10] { 0, 1, 2, 3, 0, 3, 0, 3, 1, 1 };
    //���Ӹ߶�
    static private int[] chessXSize = new int[10] { 1, 1, 1, 1, 2, 2, 2, 2, 1, 2 };
    //���ӿ��
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
                return -1; // ���������������ֲ����б��У��򷵻� -1 ��ʾ��Ч����
        }
    }

    bool MoveAllow(GameObject chessPiece, int xDir, int yDir)
    {
        int chessNum = GetChessNumber(chessPiece.name);

        //��/���ƶ�
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

            //�޸�����״̬
            for (int i = 0; i < chessYSize[chessNum]; i++)
            {
                chessBoardArray[chessXLocation[chessNum] + delta + xDir, chessYLocation[chessNum] + i] = 1;
                chessBoardArray[chessXLocation[chessNum] + delta + xDir - xDir * chessXSize[chessNum], chessYLocation[chessNum] + i] = 0;
            }

            //�޸�����λ��
            chessXLocation[chessNum] += xDir;

            return true;
        }

        //��/���ƶ�
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

            //�޸�����״̬
            for (int i = 0; i < chessXSize[chessNum]; i++)
            {
                chessBoardArray[chessXLocation[chessNum] + i, chessYLocation[chessNum] + delta + yDir] = 1;
                chessBoardArray[chessXLocation[chessNum] + i, chessYLocation[chessNum] + delta + yDir - yDir * chessYSize[chessNum]] = 0;
            }

            //�޸�����λ��
            chessYLocation[chessNum] += yDir;

            return true;
        }
        
    }

    //��ʼ���ƶ�����
    void MoveChessPieceInit(GameObject chessPiece, Vector3 playerPosition, Vector3 oldPosition, Vector3 newPosition)
    {
        
        // �������λ�õ���λ�õ�����
        Vector3 playerToOld = oldPosition - playerPosition;
        // �������λ�õ���λ�õ�����
        Vector3 playerToNew = newPosition - playerPosition;

        Vector3 myLeft = new Vector3(0.61232768f, 0f, -0.79060408f);
        Vector3 myRight = new Vector3(-0.61232768f, 0f, 0.79060408f);

        // ����ˮƽ����ļн�
        float angleHorizontal = Vector3.SignedAngle(Vector3.ProjectOnPlane(playerToOld, Vector3.up), Vector3.ProjectOnPlane(playerToNew, Vector3.up), Vector3.up);

        // ���㴹ֱ����ļн�
        float angleVertical = Vector3.SignedAngle(Vector3.ProjectOnPlane(playerToOld, myRight), Vector3.ProjectOnPlane(playerToNew, myRight), myRight);

        // ��ǰ��Ϸ���������ֵ
        float scale = 0.2f;
        // ��ǰ��Ϸ�������תֵ
        // Quaternion rotation = Quaternion.Euler(0, 52.242f, 0);
        // ���������ƶ���λ������
        // Vector3 displacement = rotation * Vector3.left * 1f * scale;

        // �ж�ˮƽ����нǺʹ�ֱ����нǵĴ�С��ϵ��ȷ�����λ��
        Debug.Log("Mathf.Abs(angleHorizontal)=" + Mathf.Abs(angleHorizontal) + ", Mathf.Abs(angleVertical)=" + Mathf.Abs(angleVertical));

        if (Mathf.Abs(angleHorizontal) > Mathf.Abs(angleVertical))
        {
            if (angleHorizontal > 0)
            {
                if(MoveAllow(chessPiece, 0, 1))
                {
                    moveFinalPosition = chessPiece.transform.position + myRight * scale; // ������ӽǵ��ұ�
                    isMoving = true;
                }

            }
            else
            {
                if (MoveAllow(chessPiece, 0, -1))
                {
                    moveFinalPosition = chessPiece.transform.position + myLeft * scale; // ������ӽǵ����
                    isMoving = true;
                }
                    
            }
        }
        else
        {
            if (angleVertical > 0)
            {
                Debug.Log(chessPiece.name+"����1��");
                if (MoveAllow(chessPiece, -1, 0))
                {
                    moveFinalPosition = chessPiece.transform.position + Vector3.down * scale; // ������ӽǵ��±�
                    isMoving = true;
                }
                    
            }
            else
            {
                Debug.Log(chessPiece.name + "����1��");
                if (MoveAllow(chessPiece, 1, 0))
                {
                    moveFinalPosition = chessPiece.transform.position + Vector3.up * scale; // ������ӽǵ��ϱ�
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
            //����triggerѡ������
            if (isPressed)
            {
                Debug.Log("ѡ��"+hit.collider.name);
                isDragging = true;
                draggableChessPiece = hit.collider.gameObject;
                dragStartPosition = draggableChessPiece.transform.position;
            }
        }


        if(isDragging)
        {

            //�ͷ�trigger�ƶ�����
            if(!isPressed)
            {
                Debug.Log("�ͷ�trigger�ƶ�����");
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
