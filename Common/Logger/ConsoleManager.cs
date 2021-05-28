using UnityEngine;
using System.Collections.Generic;
using System;
/// <summary>
/// 控制台日志
/// </summary>
/// 
/*
 * example:
        Debug.Log("dfasdf");
        Debug.LogError("dfasdf");
        mLoggerRecorder.Record(LoggerType.Normal, "adfs");
        mLoggerRecorder.Record(LoggerType.Error, "adfs");
        object a = null; string b = a.ToString();
***/
public class ConsoleManager : MonoBehaviour
{
    public bool ValidFormatLog = true;
    public bool EnableDebuger = true;

    public LoggerRecorder mLoggerRecorder;
    private List<LogEntry> loggerList = new List<LogEntry>();
    private Vector2 scroll = Vector2.zero;
    /// <summary>
    /// 是否打开控制台窗口
    /// </summary>
    public bool IsOpenWin = false;
    public bool ShowGM = false;

    void Awake()
    {
        UnityEngine.Debug.unityLogger.logEnabled = EnableDebuger;
        if (ValidFormatLog)
        {
            GameLog.AddAllLevel();
        }
#if UNITY_EDITOR
        Application.logMessageReceived += OnLog;
#endif
        mLoggerRecorder = new LoggerRecorder();
    }
    void Start()
    {
    }
    void OnLog(string text, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception || Debug.isDebugBuild)
        {
            loggerList.Add(new LogEntry(text, type));
            scroll.y = 99999f;
            if (type == LogType.Exception)
            {
                //这里记录text + "###" + stackTrace
                mLoggerRecorder.Record(LoggerType.Error, text + "\n\t\t" + stackTrace);
            }
        }
    }

    void OnGUI()
    {
        if (ShowGM)
        {
            if (GUI.Button(new Rect(0, 0, 100, 60), "CleanCache"))
            {
                CodeStage.AntiCheat.ObscuredTypes.ObscuredPrefs.DeleteAll();
            }
        }
        if (IsOpenWin == false)
        {
            return;
        }
        if (loggerList.Count == 0) return;
        scroll = GUILayout.BeginScrollView(scroll, "Box", GUILayout.Width(Screen.width/2), GUILayout.Height(Screen.height / 2));
        foreach (var entry in loggerList)
        {
            if (entry.type == LogType.Error || entry.type == LogType.Exception)
                GUI.color = Color.red;
            else if (entry.type == LogType.Warning)
                GUI.color = Color.yellow;
            GUILayout.Label(entry.text);
            GUI.color = Color.white;
        }
        GUILayout.EndScrollView();
    }
    private void OnDestroy()
    {
        //mLoggerRecorder.Dispose();
    }
}
class LogEntry
{
    public string text;
    public LogType type;
    public LogEntry(string _text, LogType _type)
    {
        text = _text;
        type = _type;
    }
}