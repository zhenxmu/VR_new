using UnityEngine;

public class CloseCanvas : MonoBehaviour
{
    public Canvas canvasToClose; // 引用要关闭的Canvas

    // 调用这个方法来关闭Canvas
    public void CloseTheCanvas()
    {
        canvasToClose.gameObject.SetActive(false);
    }
     public void OPenTheCanvas()
    {
        canvasToClose.gameObject.SetActive(true);
    }
}