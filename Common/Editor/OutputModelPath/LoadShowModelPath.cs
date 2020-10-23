using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;

namespace OutputModelPath
{
    public class LoadShowModelPath : EditorWindow
    {
        /// <summary>
        /// csv文件保存的路径
        /// </summary>
        string csvFilePath;

        /// <summary>
        /// 模型Json文件路径
        /// </summary>
        string modelJsonPath;

        DataImport dataImport = new DataImport();

        [MenuItem("Tools/加载csv文件生成模型Json路径表")]
        public static void ShowWindow()
        {
            //打开OnGUI的绘制界面
            GetWindow(typeof(LoadShowModelPath));
        }

        private void Awake()
        {
            csvFilePath = PlayerPrefs.GetString("CSVFILEPATH");
            if (string.IsNullOrEmpty(csvFilePath))
            {
                modelJsonPath = PlayerPrefs.GetString("MODELJSONPATH");
            }
        }

        private void OnDestroy()
        {
            csvFilePath = PlayerPrefs.GetString("CSVFILEPATH");
            modelJsonPath = PlayerPrefs.GetString("MODELJSONPATH");
            if (string.IsNullOrEmpty(csvFilePath)) PlayerPrefs.SetString("CSVFILEPATH", csvFilePath);
            if (string.IsNullOrEmpty(modelJsonPath)) PlayerPrefs.SetString("MODELJSONPATH", modelJsonPath);
        }

        bool isTog = false;
        private void OnGUI()
        {
            GUILayout.Space(10.0f);
            GUILayout.Label("选择csv/xlxs文件路径");
            if (GUILayout.Button("...", GUILayout.Width(30), GUILayout.Height(20)))
            {
                OpenFileDlg pth = new OpenFileDlg();
                pth.structSize = System.Runtime.InteropServices.Marshal.SizeOf(pth);
                pth.filter = "csv Files\0*.xlsx\0All Files\0*.*\0";
                pth.file = new string(new char[256]);
                pth.maxFile = pth.file.Length;
                pth.fileTitle = new string(new char[64]);
                pth.maxFileTitle = pth.fileTitle.Length;
                pth.initialDir = Application.dataPath;  // default path  
                pth.title = "打开项目";

                // 如果你保存的时候忘记加.txt时，它会自动给你加上。
                pth.defExt = ".xlsx";

                pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
                if (OpenFileDialog.GetOpenFileName(pth))
                {
                    csvFilePath = pth.file;//选择的文件路径;  
                }
            }

            //显示打开的文件路径
            GUI.TextField(new Rect(40, 30, 360, 20), csvFilePath);


            GUILayout.Label("模型Json存放路径");
            if (GUILayout.Button("...", GUILayout.Width(30), GUILayout.Height(20)))
            {

                OpenDialogDir ofn2 = new OpenDialogDir();
                ofn2.pszDisplayName = new string(new char[512]);// 存放目录路径缓冲区  
                ofn2.lpszTitle = "选择模型Json存放的文件夹";// 标题  
                                                  //ofn2.ulFlags = BIF_NEWDIALOGSTYLE | BIF_EDITBOX; // 新的样式,带编辑框  

                //打开windows系统调用打开文件夹命令
                IntPtr pidlPtr = OpenFileFolder.SHBrowseForFolder(ofn2);

                char[] charArray = new char[512];
                for (int i = 0; i < 512; i++)
                    charArray[i] = '\0';

                //保存选择文件夹的路径
                if (OpenFileFolder.SHGetPathFromIDList(pidlPtr, charArray))
                {
                    string fullDirPath = new String(charArray);
                    fullDirPath = fullDirPath.Substring(0, fullDirPath.IndexOf('\0'));
                    modelJsonPath = fullDirPath;
                }
            }

            //显示打开的文件路径
            GUI.TextField(new Rect(40, 70, 360, 20), modelJsonPath);

            GUILayout.MaxWidth(400);

            GUILayout.Space(10.0f);

            GUILayout.Label("开始导出数据，生成Json文件");
            if (GUILayout.Button("导出数据", GUILayout.Height(20)))
            {
                dataImport.ClearData();
                dataImport.ReadFile(csvFilePath);
                dataImport.SaveModelPathToLocal(modelJsonPath);
            }




            //
            GUILayout.Label("缩略图名字修改");
            if (GUILayout.Button("缩略图资源文件夹", GUILayout.Height(20)))
            {
                string path = EditorUtility.OpenFolderPanel("Load png Textures", Application.dataPath+ "/Resources/SZ/hlj", "");  //打开对应的缩略图文件夹
                string[] files = Directory.GetFiles(path,"*.png");  //获取所有文件路径   
                List<Sprite> spriteList = new List<Sprite>();
                List<string> spritePath = new List<string>();
                foreach (var item in files)
                {
                    int _index = item.IndexOf("A");
                    string _path= item.Substring(_index);
                    //Debug.Log(_path + "  ------------");
                    Sprite sp = AssetDatabase.LoadAssetAtPath<Sprite>(_path);
                    if (sp==null)
                    {
                        Debug.LogError(_path + "  不是精灵模式");
                    }
                    spriteList.Add(sp);
                    spritePath.Add(_path);
                    Debug.Log(sp);

                }
            
                Debug.Log(spriteList.Count+"   "+ dataImport.GetCataList().Count);
                List<CatalogMsg> clist= dataImport.GetCataList();
                foreach (var item in clist)
                {
                    //
                    if (string.IsNullOrEmpty(item.ModelPic))
                    {
                        continue;
                    }
                    for (int i = 0; i < spriteList.Count; i++)
                    {
                        //Debug.Log(spriteList[i] == null);
                        if (spriteList[i].name==item.ModelPic)
                        {
                            //spriteList[i].name = item.text;
                            //Debug.Log(spriteList[i].name    );
                            //EditorUtility.SetDirty(spriteList[i]);
                            //AssetDatabase.SaveAssets();
                            AssetDatabase.RenameAsset(spritePath[i], item.text);
                            EditorUtility.SetDirty(spriteList[i]);
                        }
                    }
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            
        }
    }
}