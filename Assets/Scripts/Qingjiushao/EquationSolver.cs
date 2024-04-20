using Michsky.UI.ModernUIPack;
using UnityEngine;
using UnityEngine.UI; // 引入UI命名空间
using NodeCanvas.Framework;
public class EquationSolver : MonoBehaviour
{
    public Button buttonUI1; // 按钮UI1
    public Button buttonUI2; // 按钮UI2
    public Text feedbackText; // 提示文本UI3
    public GameObject successPanel; // 成功时显示的UI元素
    public  Blackboard bb;

    private float tolerance = 0.1f; // 容忍度，决定了多接近0时算成功
    private float lowerBound = 1; // 解的下界
    private float upperBound = 4; // 解的上界

    void Start()
    {
         // 动态初始化按钮上的值
    UpdateButtonValuesRandomly();
    bb.SetVariableValue("success", false);

    // 为按钮添加点击事件监听
    buttonUI1.onClick.AddListener(() => OnButtonClicked(buttonUI1));
    buttonUI2.onClick.AddListener(() => OnButtonClicked(buttonUI2));
    }

    public void OnButtonClicked(Button clickedButton)
    {
        ButtonManagerBasicWithIcon buttonManager = clickedButton.GetComponent<ButtonManagerBasicWithIcon>();
        float value = float.Parse(buttonManager.buttonText);
        float result = CalculateEquation(value);

        if (Mathf.Abs(result) < tolerance)
        {
            feedbackText.text = "成功！接近0。";
            successPanel.SetActive(true); // 显示成功的UI元素
            bb.SetVariableValue("success",true);
        }
        else
        {
            AdjustBounds(value, result);
            UpdateButtonValuesBasedOnBounds(buttonUI1, buttonUI2);
        }
    }

    float CalculateEquation(float x)
    {
        return x * x * x - 6 * x * x + 11 * x - 6;
    }

    void UpdateButtonValuesRandomly()
    {
        float value1 = Random.Range(lowerBound, upperBound);
        float value2 = Random.Range(lowerBound, upperBound);
        UpdateButtonValues(value1, value2, buttonUI1, buttonUI2);
    }

    void AdjustBounds(float value, float result)
    {
        if (result > 0)
        {
            upperBound = value;
        }
        else
        {
            lowerBound = value;
        }
    }

    void UpdateButtonValuesBasedOnBounds(Button button1, Button button2)
    {
        float midValue = (lowerBound + upperBound) / 2;
        UpdateButtonValues(lowerBound, midValue, button1, button2);
    }

    void UpdateButtonValues(float value1, float value2, Button button1, Button button2)
    {
        ButtonManagerBasicWithIcon manager1 = button1.GetComponent<ButtonManagerBasicWithIcon>();
    ButtonManagerBasicWithIcon manager2 = button2.GetComponent<ButtonManagerBasicWithIcon>();

    manager1.buttonText = value1.ToString("F1"); // 更新按钮文本值
    manager2.buttonText = value2.ToString("F1"); // 更新按钮文本值

    manager1.UpdateUI(); // 显式更新UI显示
    manager2.UpdateUI(); // 显式更新UI显示
    }
}