using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    public Transform leftController; // Pico ??? Transform
    public InputActionReference OpenMenu;
    bool allisshow = false;
    public GameObject allobject;

    public GameObject menubt;
    public GameObject mapbt;
    public GameObject helpbt;
    public GameObject slidersbt;
    public GameObject menucontext;
    public GameObject mapcontext;
    public GameObject helpcontext;
    public GameObject sliderscontext;
    Button btn1;
    Button btn2;
    Button btn3;
    Button btn4;
    bool menuisshow = false;
    bool mapisshow = false;
    bool helpisshow = false;
    bool sliderisshow = false;

    void Start()
    {
        menucontext.SetActive(menuisshow);
        btn1 = menubt.GetComponent<Button>();
        btn1.onClick.AddListener(delegate ()
        {
            menuisshow = !menuisshow;
            menucontext.SetActive(menuisshow);
            if (menuisshow)
            {
                mapcontext.SetActive(mapisshow);
                helpcontext.SetActive(helpisshow);
                sliderscontext.SetActive(sliderisshow);
            }
            if (!menuisshow)
            {
                mapisshow = helpisshow = sliderisshow = false;
                mapcontext.SetActive(mapisshow);
                helpcontext.SetActive(helpisshow);
                sliderscontext.SetActive(sliderisshow);
            }
        });

        btn2 = mapbt.GetComponent<Button>();
        btn2.onClick.AddListener(delegate ()
        {
            mapisshow = !mapisshow;
            mapcontext.SetActive(mapisshow);
            if (mapisshow)
            {
                helpisshow = sliderisshow = false;
                helpcontext.SetActive(helpisshow);
                sliderscontext.SetActive(sliderisshow);
            }
        });

        btn3 = helpbt.GetComponent<Button>();
        btn3.onClick.AddListener(delegate ()
        {
            helpisshow = !helpisshow;
            helpcontext.SetActive(helpisshow);
            if (helpisshow)
            {
                mapisshow = sliderisshow = false;
                mapcontext.SetActive(mapisshow); 
                sliderscontext.SetActive(sliderisshow);
            }
        });

        btn4 = slidersbt.GetComponent<Button>();
        btn4.onClick.AddListener(delegate ()
        {
            sliderisshow = !sliderisshow;
            sliderscontext.SetActive(sliderisshow);
            if (sliderisshow)
            {
                mapisshow = helpisshow = false;
                mapcontext.SetActive(mapisshow); 
                helpcontext.SetActive(helpisshow);
            }
        });

        allobject.SetActive(allisshow);
    }

    void OnEnable()
    {
        OpenMenu.action.Enable();
        OpenMenu.action.performed += OnMenuEntered;
    }

    void OnDisable()
    {
        OpenMenu.action.Disable();
        OpenMenu.action.performed -= OnMenuEntered;
    }

    private void OnMenuEntered(InputAction.CallbackContext context)
    {
        allisshow = !allisshow;
        allobject.SetActive(allisshow);
    }
}