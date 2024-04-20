using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DebugTextDisplay : MonoBehaviour
{
    private Text debugText;
    private List<string> logList = new List<string>(); // 用于保存日志信息的列表
    private int maxLogs = 2; // 最大保留的日志条数

    void Start()
    {
        // 获取 Text 组件
        debugText = GetComponent<Text>();

        if (debugText == null)
        {
            Debug.LogError("DebugTextDisplay脚本需要挂载在Text组件上！");
        }

        // 订阅Unity的日志回调事件
        Application.logMessageReceived += HandleLog;
    }

    void Update()
    {
        // 更新UI Text显示
        if (debugText != null)
        {
            UpdateDebugText();
        }
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // 处理日志信息
        logList.Add(logString);

        // 超出最大保留条数时移除最早的日志信息
        if (logList.Count > maxLogs)
        {
            logList.RemoveAt(0);
        }
    }

    void UpdateDebugText()
    {
        // 更新UI Text显示
        string displayText = "Debug信息：\n"; // 初始输出

        foreach (string log in logList)
        {
            displayText += log + "\n";
        }

        debugText.text = displayText;
    }
}
