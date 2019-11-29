using huang.common.paths;
using System;
using System.Diagnostics;
using System.IO;

namespace F3D
{
    /// <summary>
    /// 日志管理类
    /// </summary>
    public class AppLog
    {
        /// <summary>
        /// 日志文件的保存路径
        /// </summary>
        public static string StrLogPath = AppDomain.CurrentDomain.BaseDirectory + "AppLog";
        /// <summary>
        /// 日志文件的全路径
        /// </summary>
        public static string StrLogFile = AppDomain.CurrentDomain.BaseDirectory + @"AppLog\ErrorLog.txt";
        /// <summary>
        /// 是否按小时拆分日志
        /// </summary>
        public static bool IsHourSplit = true;

        /// <summary>
        /// 初始化文件路径信息
        /// </summary>
        public static string GetFilePath()
        {
            try
            {
                // 计算新的日志文件
                //string path = AppDomain.CurrentDomain.BaseDirectory + "AppLog\\";
                string path = Paths.StartupPath + "\\" + "AppLog\\";
                DateTime time = DateTime.Now;
                string strDate = time.ToShortDateString();
                strDate = strDate.Replace("\\", "-");
                strDate = strDate.Replace("/", "-");
                path += strDate;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (IsHourSplit)
                    path = path + "\\Log_" + time.Hour + ".txt";
                else
                    path = path + "\\Log.txt";
                if (!File.Exists(path))
                {
                    using (StreamWriter writer = File.CreateText(path))
                    {
                        writer.WriteLine("程序运行日志：\r\n");
                    }
                }
                return path;
            }
            catch 
            {
            }
            return StrLogFile;
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exp"></param>
        public static void AddErrorLog(string message, Exception exp)
        {
            Debug.WriteLine(message);
            try
            {
                string path = GetFilePath();
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(DateTime.Now.ToString());
                    writer.WriteLine(message);
                    writer.WriteLine(exp.Message);
                    writer.WriteLine(exp.ToString());
                    if (exp.InnerException != null)
                    {
                        writer.WriteLine(exp.InnerException.Message);
                        writer.WriteLine(exp.InnerException.TargetSite);
                    }
                    writer.WriteLine("\r");
                }
                Debug.WriteLine(DateTime.Now.ToString());
                Debug.WriteLine(message);
                Debug.WriteLine(exp.ToString());
                if (exp.InnerException != null)
                {
                    Debug.WriteLine(exp.InnerException.ToString());
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="exp"></param>
        public static void AddException(Exception exp)
        {
            try
            {
                string path = GetFilePath();
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(DateTime.Now.ToString());
                    writer.WriteLine(exp.Message);
                    writer.WriteLine(exp.ToString());
                    if (exp.InnerException != null)
                    {
                        writer.WriteLine(exp.InnerException.Message);
                        writer.WriteLine(exp.InnerException.TargetSite);
                    }
                    writer.WriteLine("\r");
                }
                Debug.WriteLine(DateTime.Now.ToString());
                Debug.WriteLine(exp.ToString());
                if (exp.InnerException != null)
                {
                    Debug.WriteLine(exp.InnerException.ToString());
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 记录程序日志信息
        /// </summary>
        /// <param name="message"></param>
        public static void AddMsg(string message)
        {
            //Debug.WriteLine(message);
//#if UNITY_EDITOR
            UnityEngine.Debug.Log(message);
//#endif
            try
            {
                string path = GetFilePath();
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(DateTime.Now.ToString());
                    writer.WriteLine(message);
                    writer.WriteLine("\r");
                }
                //Debug.WriteLine(DateTime.Now.ToString());
                //Debug.WriteLine(message);
            }
            catch
            {
            }
        }
    }
}

