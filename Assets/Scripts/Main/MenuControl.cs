using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
public class MenuControl : MonoBehaviour
{

    public InputActionReference OpenMenu;
    public Transform playPlacement;
    public Transform uiPlace;
    public float MenuUIDIstance;
    private bool isMenuOpened=false;
    // Start is called before the first frame update
    void Start()
    {
        uiPlace.gameObject.SetActive(false);
        OpenMenu.action.performed += OnMenuEnterd;
    }
    
    private void OnMenuEnterd(InputAction.CallbackContext obj)
    {
        isMenuOpened=!isMenuOpened;
        uiPlace.gameObject.SetActive(isMenuOpened);
        if(isMenuOpened)
        {
            uiPlace.position=playPlacement.position+Vector3.forward*MenuUIDIstance;
            uiPlace.RotateAround(playPlacement.position,Vector3.up,playPlacement.rotation.eulerAngles.y);

        }else{
            uiPlace.rotation=Quaternion.Euler(Vector3.zero);
        }
    }
    
}
