using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class PrefabCreatedCopy  {
    
    [MenuItem("Assets/创建预制体到书中")]
    static void Created()
    {
        GameObject go = Selection.activeObject as GameObject;

        string parName= GUIUtility.systemCopyBuffer;
        Debug.Log("剪切板内容: " + parName);

        GameObject par= new GameObject(parName);
        par.transform.localPosition = Vector3.zero;

        go = PrefabUtility.InstantiateAttachedAsset(go) as GameObject;
        go.name= go.name.Split('(')[0];
        go.transform.SetParent(par.transform);
        go.transform.localPosition = Vector3.zero;
        //go.transform.localScale = Vector3.one;

        string savePath =Application.dataPath+"/"+ parName;
        int _index = savePath.LastIndexOf("_");
        savePath= savePath.Substring(0, _index);
        savePath= savePath.Replace("_", "/");
        Debug.Log(savePath);
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        Debug.Log("dataPath: "+Application.dataPath);
        savePath =savePath.Remove(0,savePath.LastIndexOf("Assets"));

        Debug.Log("savePath:  "+savePath);
        string prefabSavePath = savePath +"/"+parName+".prefab";
        Debug.Log("预制体保存路径 "+prefabSavePath);


        GameObject prefab = PrefabUtility.CreatePrefab(prefabSavePath, par);
        PrefabUtility.ConnectGameObjectToPrefab(par, prefab);
        //ModelControl control= prefab.GetComponent<ModelControl>();
        //if (control==null)
        //{
        //    prefab.AddComponent<ModelControl>();
        //}

        EditorGUIUtility.PingObject(par);//设置焦点
        Selection.activeGameObject = par;

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

    }

    static List<string> matList = new List<string>();

    [MenuItem("Tools/获取模型中的材质列表")]
    static void GetMatList()
    {
        matList.Clear();
        GameObject go = Selection.activeGameObject;
        Debug.Log("选中的obj    "+go.name);
        Renderer[] renders=go.GetComponentsInChildren<Renderer>(true);        
        foreach (Renderer item in renders)
        {
            string log = item.name + "----------\n";
            Material[] _mats = item.sharedMaterials;
            foreach (var _item in _mats)
            {
                log += item.name + "\n";
                matList.Add(_item.name);
            }
            Debug.Log(log);
           
        }        
    }

    [MenuItem("Tools/设置模型中的材质为Fade(alpha=70)")]
    static void SetMats()
    {
        Object[] objs=Selection.objects;
        string path= AssetDatabase.GetAssetPath(objs[0]);        
        string dirPath =Path.GetDirectoryName(path);
        //dirPath= EditorUtility.OpenFolderPanel("材质球文件夹", dirPath, "文件夹");
        Debug.Log("目录路径  :"+dirPath);

        //for (int i = 0; i < objs.Length; i++)
        //{
        //    if (matList.Contains(objs[i].name))
        //    {
        //        matList.Remove(objs[i].name);
        //        Debug.Log("移除的mat  " + objs[i].name);
        //    }
        //}
        //return;

        #region 材质测试
        //path = path.Replace("\\", "/");
        //Debug.Log(path);
        //Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        //mat.SetFloat("_Mode", 2);
        //Color32 c = mat.color;
        //c.a = 70;
        //mat.SetColor("_Color", c);
        //UnityEditor.EditorUtility.SetDirty(mat);
        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();
        //return;
        #endregion

        string[] matsPath = new string[matList.Count];
        for (int i = 0; i < matsPath.Length; i++)
        {            
            matsPath[i] = Path.Combine(dirPath, matList[i]);
            matsPath[i] = matsPath[i].Replace("\\", "/");
            Debug.Log(matsPath[i]);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(matsPath[i] + ".mat");          
            mat.SetFloat("_Mode", 2);
            Color32 c = mat.color;
            c.a = 70;
            mat.SetColor("_Color", c);
            UnityEditor.EditorUtility.SetDirty(mat);
            AssetDatabase.IsOpenForEdit(mat, StatusQueryOptions.ForceUpdate);
            //AssetDatabase.ImportAsset(matsPath[i] + ".mat", ImportAssetOptions.ForceUpdate);          


        }     
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

 
    //设置材质的渲染模式
    static void setMaterialRenderingMode(Material material, RenderingMode renderingMode)
    {
        switch (renderingMode)
        {
            case RenderingMode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderingMode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case RenderingMode.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case RenderingMode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }
}
public enum RenderingMode
{
    Opaque,
    Cutout,
    Fade,
    Transparent
}
