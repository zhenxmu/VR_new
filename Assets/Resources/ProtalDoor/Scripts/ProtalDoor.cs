using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(Collider), typeof(MeshRenderer))]
public class ProtalDoor : MonoBehaviour
{
    [SerializeField]
    private LayerMask TransformTargetLayer = 0;

    public ProtalDoor TargetDoor = null;

    private Transform m_ProtalCamera = null;
    private Transform m_MainCameraPos = null;
    private Transform m_TargetPos = null;
    private Transform m_Camera = null;

    public RenderTexture ProtalCameraTexture { get; private set; } = null;
    
    private void Awake()
    {
        m_ProtalCamera = transform.Find("ProtalCamera");
        m_MainCameraPos = transform.Find("MainCameraPos");
        m_TargetPos = transform.Find("TargetPos");
        m_Camera = Camera.main.transform;

        Camera cam = m_ProtalCamera.GetComponent<Camera>();
        ProtalCameraTexture = new RenderTexture(Screen.width, Screen.height, 32)
        {
            name = "ProtalCameraTargetTexture"
        };
        cam.targetTexture = ProtalCameraTexture;
        cam.cullingMask &= ~(1 << gameObject.layer);
    }

    private void Start()
    {
        if( TargetDoor != null )
        {
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            foreach (var m in renderer.materials)
            {
                m.SetTexture("_MainTex", TargetDoor.ProtalCameraTexture);
            }
        }
    }

    private void Update()
    {
        if( TargetDoor != null )
        {
            m_MainCameraPos.position = m_Camera.position;
            m_MainCameraPos.rotation = m_Camera.rotation;
            TargetDoor.m_ProtalCamera.localPosition = m_MainCameraPos.localPosition;
            TargetDoor.m_ProtalCamera.localRotation = m_MainCameraPos.localRotation;
        }
    }

    private bool bAcceptTrans = true;
   private void OnTriggerEnter(Collider other)
{
    if (bAcceptTrans && (((1 << other.gameObject.layer) & TransformTargetLayer) != 0))
    {
        TargetDoor.bAcceptTrans = false;
        Debug.Log("触发传送器");

        m_TargetPos.position = other.transform.position;
        m_TargetPos.rotation = other.transform.rotation;

        // 在这里添加180度的旋转
        Quaternion rotationAdjustment = Quaternion.Euler(0, 180, 0);
        TargetDoor.m_TargetPos.localPosition = m_TargetPos.localPosition;
        TargetDoor.m_TargetPos.localRotation = m_TargetPos.localRotation * rotationAdjustment;
      
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null)
            cc.enabled = false;
        other.transform.position = TargetDoor.m_TargetPos.position;
        other.transform.rotation = TargetDoor.m_TargetPos.rotation;
        Debug.Log(other.transform.position);
         

        if (cc != null)
            cc.enabled = true;
    }
}

    private void OnTriggerExit(Collider other)
    {
        if (!bAcceptTrans)
            bAcceptTrans = true;
    }
}
