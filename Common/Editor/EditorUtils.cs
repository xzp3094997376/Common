using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;
public class EditorUtils : Editor {
    
    static void CheckMissingScript()
    {
        GameObject pObjectParent = Selection.activeGameObject;
        Transform[] transforms = pObjectParent.transform.GetComponentsInChildren<Transform>(true);
        for(int i = 0; i < transforms.Length; i++)
        {
            GameObject pObject = transforms[i].gameObject;
            var components = pObject.GetComponents<Component>();
            for(int j = 0; j < components.Length; j++)
            {
                if(components[j] == null)
                {
                    Debug.LogError("存在脚本丢失 ： " + pObject.name);
                }
            }
        }
    }

    

    [MenuItem("Tools/删除预制体丢失脚本")]
    public static void CleanupMissingScript()
    {
        GameObject select = Selection.activeGameObject;
        if (select == null)
        {
            Debug.LogError("异常");
            return;
        }
        int r;
        int j;
        Transform[] list = select.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].gameObject.hideFlags == HideFlags.None)//HideFlags.None 获取Hierarchy面板所有Object
            {
                var components = list[i].gameObject.GetComponents<Component>();
                var serializedObject = new SerializedObject(list[i].gameObject);
                var prop = serializedObject.FindProperty("m_Component");
                r = 0;
                for (j = 0; j < components.Length; j++)
                {
                    if (components[j] == null)
                    {
                        prop.DeleteArrayElementAtIndex(j - r);
                        r++;
                    }
                }
                serializedObject.ApplyModifiedProperties();
            }
        }
        Debug.Log("CleanupMissingScript : " + select.name);

    }


    [MenuItem("GameObject/删除预制体Mono脚本",priority = -20)]
    public static void CleanupMomoScript()
    {
        GameObject select = Selection.activeGameObject;
        if (select == null)
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(select);
            Debug.LogError("异常");
            return;
        }
#if UNITY_2018
      
        int r;
        int j;
        Transform[] list = select.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].gameObject.hideFlags == HideFlags.None)//HideFlags.None 获取Hierarchy面板所有Object
            {
                var components = list[i].gameObject.GetComponents<Component>();
                var serializedObject = new SerializedObject(list[i].gameObject  );
                var prop = serializedObject.FindProperty("m_Component");
                r = 0;
                for (j = 0; j < components.Length; j++)
                {
                    if (components[j] is MonoBehaviour)
                    {
                        prop.DeleteArrayElementAtIndex(j - r);
                        r++;
                    }
                }
                serializedObject.ApplyModifiedProperties();
            }
        }
#else
        MonoBehaviour[] list = select.GetComponentsInChildren<MonoBehaviour>(true);
        foreach (var item in list)
        {
            if (item is UnityEngine.EventSystems.UIBehaviour)
            {
                continue;
            }
            DestroyImmediate(item);
            Debug.Log(item.GetType().Name);
        }

        string assetPath = AssetDatabase.GetAssetPath(select);
        assetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(select);
        Debug.Log(assetPath);
        //bool suc;
        PrefabUtility.prefabInstanceUpdated = (go) =>
        {
            Debug.Log("预制体更新: " + go.name);
        };

        PrefabUtility.ApplyObjectOverride(select, assetPath, InteractionMode.UserAction);
        PrefabUtility.ApplyPrefabInstance(select, InteractionMode.UserAction);
        PrefabUtility.SavePrefabAsset(select);
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);


#endif
        Debug.Log("CleanupMissingScript : " + select.name);

    }
        

    [MenuItem("GameObject/添加符号节点", false, 0)]
    public static void ModelFlashPlayEditor()
    {
        GameObject select = Selection.activeGameObject;
        if (select == null)
        {
            Debug.LogError("异常");
            return;
        }

        select.GetComponent<Debug_ModelFlashPlay>().TestFuHao();

    }

    //[MenuItem("Assets/转换TXT文档编码(utf8)")]
    static void ChangeTxtEncoded()
    {
        UnityEngine.Object obj = Selection.activeObject;
        if(obj == null)
        {
            Debug.LogError("选择文件异常");
        }
        string path = obj.name;
        Debug.Log("需要转换格式的教材名称缩写： " + path);

        string root = Application.dataPath;
        string temp = root.Replace("/Assets", "");
        int last = temp.LastIndexOf("/");
        string roottxt = temp.Substring(0, last + 1) + "ResTxts";
        Debug.Log("TXT文档根目录： " + roottxt);
        string fullpath = roottxt + "/" + obj.name;
        string[] files = Directory.GetFiles(fullpath, "*.txt");
        for(int i = 0; i < files.Length; i++)
        {
            Encoding en = GetType(files[i]);
            if(en == Encoding.UTF8)
            {
                Debug.Log("utf8 " + Path.GetFileName(files[i]));
                continue;
            }
            else
            {

                string content = File.ReadAllText(files[i], Encoding.Unicode);
                Debug.Log(content);
                File.WriteAllText(files[i], content, Encoding.UTF8);
            }
        }
        Debug.Log("转换编码完成！");
    }

    /// <summary>
    /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型
    /// </summary>
    /// <param name=“FILE_NAME“>文件路径</param>
    /// <returns>文件的编码类型</returns>
    public static System.Text.Encoding GetType(string FILE_NAME)
    {
        FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);

        Encoding r = GetType(fs);
        fs.Close();
        return r;
    }

    /// <summary>
    /// 通过给定的文件流，判断文件的编码类型
    /// </summary>
    /// <param name=“fs“>文件流</param>
    /// <returns>文件的编码类型</returns>
    public static System.Text.Encoding GetType(FileStream fs)
    {
        byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
        byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
        byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM
        Encoding reVal = Encoding.Default;

        BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
        int i;
        int.TryParse(fs.Length.ToString(), out i);
        byte[] ss = r.ReadBytes(i);
        if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
        {
            reVal = Encoding.UTF8;
        }
        else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
        {
            reVal = Encoding.BigEndianUnicode;
        }
        else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
        {
            reVal = Encoding.Unicode;
        }
        r.Close();
        return reVal;

    }

    /// <summary>
    /// 判断是否是不带 BOM 的 UTF8 格式
    /// </summary>
    /// <param name=“data“></param>
    /// <returns></returns>
    private static bool IsUTF8Bytes(byte[] data)
    {
        int charByteCounter = 1; //计算当前正分析的字符应还有的字节数
        byte curByte; //当前分析的字节.
        for (int i = 0; i < data.Length; i++)
        {
            curByte = data[i];
            if (charByteCounter == 1)
            {
                if (curByte >= 0x80)
                {
                    //判断当前
                    while (((curByte <<= 1) & 0x80) != 0)
                    {
                        charByteCounter++;
                    }
                    //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X
                    if (charByteCounter == 1 || charByteCounter > 6)
                    {
                        return false;
                    }
                }
            }
            else
            {
                //若是UTF-8 此时第一位必须为1
                if ((curByte & 0xC0) != 0x80)
                {
                    return false;
                }
                charByteCounter--;
            }
        }
        if (charByteCounter > 1)
        {
            throw new Exception("非预期的byte格式");
        }
        return true;
    }
}
