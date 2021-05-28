using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggerRecorder
{
    private ILogger logger = Debug.unityLogger;
    public string LoggerTag = "Logger";
    private FileLogger mFileLogger;
    private object obj;

    public LoggerRecorder()
    {
        mFileLogger = new FileLogger();
    }
    /// <summary>
    /// 记录日志
    /// </summary>
    /// <param name="type">日志等级，是否记录取决于设置的日志过滤类型</param>
    /// <param name="context">这条记录来自于哪个对象</param>
    /// <param name="content">日志内容</param>
    public void Record(LoggerType type, object context, string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return;
        }
        string className = content != null ? context.GetType().FullName : "null";
        string logStr = string.Format("[{0}@{1}#{2}],{3}", type, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), className, content);
        LoggerHandler(type, logStr);
    }
    /// <summary>
    /// 记录
    /// </summary>
    /// <param name="type">日志等级，是否记录取决于设置的日志过滤类型</param>
    /// <param name="content">日志内容</param>
    public void Record(LoggerType type, string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return;
        }
        string logStr = string.Format("[{0}@{1}]{2}", type, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), content);
        LoggerHandler(type, logStr);
    }
    private void LoggerHandler(LoggerType type, string logStr)
    {
        if (type == LoggerType.Normal)
        {
            logger.Log(LoggerTag, logStr);
        }
        else if (type == LoggerType.Warning)
        {
            logger.LogWarning(LoggerTag, logStr);
        }
        else if (type == LoggerType.Error)
        {
            logger.LogError(LoggerTag, logStr);
        }
    }
    public void Dispose()
    {
        //mFileLogger.Dispose();
    }
}
public enum LoggerType
{
    Normal,
    Warning,
    Error,
    //Exception,
}