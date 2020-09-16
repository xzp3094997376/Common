using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;

public class FindChineseTool : MonoBehaviour
{
    [MenuItem("Language/查找代码中文")]
    public static void Pack()
    {
        Rect wr = new Rect(300, 400, 400, 100);
        FindChineseWindow window = (FindChineseWindow)EditorWindow.GetWindowWithRect(typeof(FindChineseWindow), wr, true, "查找项目中的中文字符");
        window.Show();
    }
}

public class FindChineseWindow : EditorWindow
{
    private ArrayList csList = new ArrayList();
    private int eachFrameFind = 4;
    private int currentIndex = 0;
    private bool isBeginUpdate = false;
    private string outputText;
    public string filePath = ""; 

    private void Awake()
    {
    }

    private void GetAllFIle(DirectoryInfo dir)
    {
        FileInfo[] allFile = dir.GetFiles();
        foreach (FileInfo fi in allFile)
        {
            //if (fi.DirectoryName.IndexOf("\\Assets\\Develop") == -1)//排除指定路径下的代码
            //    continue;
            if (fi.DirectoryName.Contains("FindChineseTool"))//排除指定名称的代码
                continue;
            if (fi.FullName.IndexOf(".meta") == -1 && fi.FullName.IndexOf(".cs") != -1)
            {
                csList.Add(fi.DirectoryName + "/" + fi.Name);
            }
        }
        DirectoryInfo[] allDir = dir.GetDirectories();
        foreach (DirectoryInfo d in allDir)
        {
            GetAllFIle(d);
        }
    }

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextField(filePath);
        //Debug.Log("&& 图片名字:"+ str);
        if (GUILayout.Button("粘贴", GUILayout.Width(100)))
        {
            TextEditor te = new TextEditor();
            te.Paste();
            filePath = te.text;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(outputText, EditorStyles.boldLabel);

        if (GUILayout.Button("开始遍历项目"))
        {
            csList.Clear();
            DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "/" + filePath);
            GetAllFIle(dirInfo);
            GetAllFIle(dirInfo);
            outputText = "游戏内代码文件的数量：" + csList.Count;
            isBeginUpdate = true;
            outputText = "开始遍历项目";
        }

        EditorGUILayout.EndHorizontal();
    }

    void Update()
    {
        if (isBeginUpdate && currentIndex < csList.Count)
        {
            int count = (csList.Count - currentIndex) > eachFrameFind ? eachFrameFind : (csList.Count - currentIndex);
            for (int i = 0; i < count; i++)
            {
                string url = csList[currentIndex].ToString();
                currentIndex = currentIndex + 1;
                url = url.Replace("\\", "/");
                printChinese(url);
            }
            if (currentIndex >= csList.Count)
            {
                isBeginUpdate = false;
                currentIndex = 0;
                outputText = "遍历结束，总共" + csList.Count;
            }
        }
    }

    private bool HasChinese(string str)
    {
        return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
    }

    private Regex regex = new Regex("\"[^\"]*\"");
    private void printChinese(string path)
    {
        if (File.Exists(path))
        {
            string[] fileContents = File.ReadAllLines(path, Encoding.Default);
            int count = fileContents.Length;
            for (int i = 0; i < count; i++)
            {
                string printStr = fileContents[i].Trim();

                if (printStr.IndexOf("//") == 0)  //说明是注释
                    continue;
                if (printStr.IndexOf("Debug.Log") == 0)  //说明是注释
                    continue;
                //if (printStr.IndexOf("ALog.Log") == 0)  //说明是注释
                //    continue;
                MatchCollection matches = regex.Matches(printStr);
                foreach (Match match in matches)
                {
                    if (HasChinese(match.Value))
                    {
                        Debug.Log("路径:" + path + " 行数:" + i + " 内容:" + printStr);
                        UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(path, i-1);
                        break;
                    }
                }
            }
            fileContents = null;
        }
    }
}