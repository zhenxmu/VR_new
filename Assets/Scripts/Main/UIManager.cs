using TMPro;
using UnityEngine;
using UnityEngine.UI; // 如果你使用的是Text
// using TMPro; // 如果你使用的是TextMeshPro

public class UIManager : MonoBehaviour
{
    public GameObject popup; // 引用弹窗提示的GameObject
    
    public TextMeshProUGUI popupText; // 如果你使用的是TextMeshPro
   
    public TextMeshProUGUI menuText; // 如果你使用的是TextMeshPro

    // 显示弹窗提示并更新菜单界面的文字
    public void ShowMessage(string message)
    {
        // 显示弹窗提示
        popup.SetActive(true);
        popupText.text = message;
        // 更新菜单界面的文字
        menuText.text += message + "\n"; // 换行显示新的消息

        // 可以在这里设置一个计时器，几秒后自动隐藏弹窗提示
        Invoke("HidePopup", 5); // 5秒后隐藏弹窗
    }

    // 隐藏弹窗提示
    void HidePopup()
    {
        popup.SetActive(false);
    }
}