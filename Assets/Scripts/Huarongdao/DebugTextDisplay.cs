using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DebugTextDisplay : MonoBehaviour
{
    private Text debugText;
    private List<string> logList = new List<string>(); // ���ڱ�����־��Ϣ���б�
    private int maxLogs = 2; // ���������־����

    void Start()
    {
        // ��ȡ Text ���
        debugText = GetComponent<Text>();

        if (debugText == null)
        {
            Debug.LogError("DebugTextDisplay�ű���Ҫ������Text����ϣ�");
        }

        // ����Unity����־�ص��¼�
        Application.logMessageReceived += HandleLog;
    }

    void Update()
    {
        // ����UI Text��ʾ
        if (debugText != null)
        {
            UpdateDebugText();
        }
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // ������־��Ϣ
        logList.Add(logString);

        // �������������ʱ�Ƴ��������־��Ϣ
        if (logList.Count > maxLogs)
        {
            logList.RemoveAt(0);
        }
    }

    void UpdateDebugText()
    {
        // ����UI Text��ʾ
        string displayText = "Debug��Ϣ��\n"; // ��ʼ���

        foreach (string log in logList)
        {
            displayText += log + "\n";
        }

        debugText.text = displayText;
    }
}
