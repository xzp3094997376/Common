#if UNITY_EDITOR
#define DEBUG
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

/**日志接口**/

public class GameLog
{

    public static readonly IList<LogType> LevelList = new List<LogType>() { };

    /**打印方式[true:控制台打印,false:Unity Debug打印]**/
    public static bool PrintWay { get; set; }

    /**添加打印级别**/
    public static void AddLevel(LogType level)
    {
        if (level > 0) LevelList.Add(level);
    }
    public static void AddAllLevel()
    {
        LevelList.Clear();
        LevelList.Add(LogType.Error);
        LevelList.Add(LogType.Exception);
        LevelList.Add(LogType.Log);
        LevelList.Add(LogType.Warning);
    }
    public static void ClearLog()
    {
        LevelList.Clear();
    }

    private static void Log(String name, object log)
    {
        if (LevelList.IndexOf(LogType.Log) != -1)
        {
            Format(name, LogType.Log, log);
        }
    }
    [Conditional("DEBUG")]
    public static void LogInfo(object obj, object log, string param)
    {
        if (obj is string)
        {
            Log(obj as string, string.Format((string)log, param));
        }
        else
        {
            Log(obj.GetType().FullName, log);
        }
    }
    [Conditional("DEBUG")]
    public static void LogInfo(object obj, object log)
    {
        if (obj is string)
        {
            Log(obj as string, log);
        }
        else
        {
            Log(obj.GetType().FullName, log);
        }
    }

    private static void Exception(String name, String log)
    {
        if (LevelList.IndexOf(LogType.Exception) != -1)
        {
            Format(name, LogType.Exception, log);
        }
    }

    [Conditional("DEBUG")]
    public static void ExceptionInfo(object obj, String log)
    {
        if (obj is string)
        {
            Exception(obj as string, log);
        }
        else
        {
            Exception(obj.GetType().FullName, log);
        }
    }

    private static void Warning(String name, object log)
    {
        if (LevelList.IndexOf(LogType.Warning) != -1)
        {
            Format(name, LogType.Warning, log);
        }
    }

    [Conditional("DEBUG")]
    public static void WarningInfo(object obj, object log)
    {
        if (obj is string)
        {
            Warning(obj as string, log);
        }
        else
        {
            Warning(obj.GetType().FullName, log);
        }
    }

    private static void Error(String name, object log)
    {
        if (LevelList.IndexOf(LogType.Error) != -1)
        {
            Format(name, LogType.Error, log);
        }
    }
    [Conditional("DEBUG")]
    public static void ErrorInfo(object obj, object log, string param)
    {
        if (obj is string)
        {
            Error(obj as string, string.Format((string)log, param));
        }
        else
        {
            Error(obj.GetType().FullName, log);
        }
    }
    [Conditional("DEBUG")]
    public static void ErrorInfo(object obj, object log)
    {
        if (obj is string)
        {
            Error(obj as string, log);
        }
        else
        {
            Error(obj.GetType().FullName, log);
        }
    }
    public static string LogTypeTitle(LogType level)
    {
        switch (level)
        {
            case LogType.Error:
                return "<color=#FF0000>[错误]</color>";
            case LogType.Warning:
                return "<color=#FFFF00>[警告]</color>";
            case LogType.Log:
                default:
                return "<color=#00FF00>[日志]</color>";
            case LogType.Exception:
                return "<color=#00FFFF>[异常]</color>";
        }
    }
    private static void Format(String name, LogType level, object log)
    {
        var str = new StringBuilder();
        str.Append(LogTypeTitle(level));
        str.Append(" " + log.ToString());
        str.Append(" [" + name + "]");
        str.Append(" [" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + DateTime.Now.Millisecond + "] ");

        if (PrintWay) Console.WriteLine(str);
        else
        {
            if (level.Equals(LogType.Error.ToString()))
            {
                UnityEngine.Debug.LogError(str);
            }
            else if (level.Equals(LogType.Warning.ToString()))
            {
                UnityEngine.Debug.LogWarning(str);
            }
            else
            {
                UnityEngine.Debug.Log(str);
            }
        }
    }
}