#if DEBUG_DEVELOP_AR_MODEL && (UNITY_STANDALONE || UNITY_EDITOR)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class AssetbundleDebugLoad : EditorWindow
{
    string path;
    Rect rect;

    [MenuItem("Window/AB_DebugLoad")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(AssetbundleDebugLoad));
    }
    string Debug_abFullPath = "";
    void OnGUI()
    {
        EditorGUILayout.LabelField("路径");
        //获得一个长300的框
        rect = EditorGUILayout.GetControlRect(GUILayout.Width(300), GUILayout.Height(100));
        //将上面的框作为文本输入框
        path = EditorGUI.TextField(rect, path);

        //如果鼠标正在拖拽中或拖拽结束时，并且鼠标所在位置在文本输入框内
        if ((Event.current.type == EventType.DragUpdated
          || Event.current.type == EventType.DragExited)
          && rect.Contains(Event.current.mousePosition))
        {
            //改变鼠标的外表
            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
            {
                path = DragAndDrop.paths[0];
                Debug_abFullPath = path;
                if (Event.current.type == EventType.DragExited)
                {
                    Debug.Log("EventType.DragExited");
                    Debug.Log("Load AB : " + Debug_abFullPath);
                    Common._instance.ModelPreviewBack();
                    MobileMutualControl.instance.ResourceLoadMode("AB");
                    MobileMutualControl.instance.StartModelPreview(System.IO.Path.GetFileNameWithoutExtension(Debug_abFullPath) + "|" + Debug_abFullPath);
                }
            }
        }
    }
}
#endif