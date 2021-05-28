using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class FileLogger : ILogHandler
{
    private ILogger logger = UnityEngine.Debug.unityLogger;
    private FileStream m_FileStream;
    private StreamWriter m_StreamWriter;
    private ILogHandler m_DefaultLogHandler = UnityEngine.Debug.unityLogger.logHandler;

    public FileLogger()
    {
        if (Debug.isDebugBuild)
            logger.filterLogType = LogType.Log;
        else
            logger.filterLogType = LogType.Error;
        //C:\Users\Administrator\AppData\LocalLow\CompanyName\ProductName
        string fileFolder = Directory.GetCurrentDirectory() + "/Log/";
        string filePath = fileFolder +
            string.Format("/{0}_CustomLogs.txt", GameLogic.Instance.currCourseSoft.ToString());
        if (!Directory.Exists(fileFolder))
        {
            Directory.CreateDirectory(fileFolder);
        }
        m_FileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        m_StreamWriter = new StreamWriter(m_FileStream);
        logger.logHandler = this;
        Application.logMessageReceived += logMessageReceived;

    }
    private void logMessageReceived(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            string logStr = string.Format("[{0}@{1}]{2}",
            type,
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            string.Format("{0}#{1}", condition, stackTrace)
            );
            m_StreamWriter.WriteLine(logStr);
        }
    }
    /// <summary>
    /// 记录自定义日志和Debug.Log/LogWarning...日志
    /// </summary>
    /// <param name="logType"></param>
    /// <param name="context"></param>
    /// <param name="format"></param>
    /// <param name="args"></param>
    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        m_StreamWriter.WriteLine(String.Format(format, args));
        m_StreamWriter.Flush();
        m_DefaultLogHandler.LogFormat(logType, context, format, args);
    }

    public void LogException(Exception exception, UnityEngine.Object context)
    {
        m_DefaultLogHandler.LogException(exception, context);
    }
    public void Dispose()
    {
        Application.logMessageReceived -= logMessageReceived;
        m_StreamWriter.Close();
        m_StreamWriter.Dispose();

    }
}
