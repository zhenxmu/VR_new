using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowManager : MonoBehaviour
{
    public InputActionReference toggleArrowsAction; // 用于切换箭头显示的输入动作引用
    public GameObject[] arrows; // 存储所有箭头对象的数组
    private bool areArrowsVisible = true; // 跟踪箭头的当前显示状态

    void Start()
    {
        // 注册输入动作的执行事件
        toggleArrowsAction.action.performed += ToggleArrowsVisibility;
    }

    void OnEnable()
    {
        // 启用输入动作
        toggleArrowsAction.action.Enable();
    }

    void OnDisable()
    {
        // 禁用输入动作
        toggleArrowsAction.action.Disable();
    }

    private void ToggleArrowsVisibility(InputAction.CallbackContext context)
    {
        // 切换箭头的显示状态
        areArrowsVisible = !areArrowsVisible;

        // 根据当前状态显示或隐藏所有箭头
        foreach (var arrow in arrows)
        {
            if (arrow != null)
            {
                arrow.SetActive(areArrowsVisible);
            }
        }
    }
}