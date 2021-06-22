#if UNITY_STANDALONE || UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class EditorCopyNodePath : Editor
{
    [MenuItem("GameObject/Copy Path",priority = -2)]
    public static void CopyNodePathWithoutRoot()
    {
        GameObject node = Selection.activeGameObject;
        if(node == null)
        {
            Debug.LogError("请选择模型节点!");
            return;
        }
        string nodePath = node.name;
        Transform[] _parents = node.GetComponentsInParent<Transform>(true);
        if(_parents.Length < 2) // node为root
        {
            return;
        }
        if (_parents.Length == 2) // node为root下第一层级的节点，
        {
            GUIUtility.systemCopyBuffer = nodePath;
            return;
        }
        for(int i = 1; i < _parents.Length - 1; i++)
        {
            //Debug.Log("Node: " + _parents[i].gameObject.name);
            nodePath = _parents[i].gameObject.name + "/" + nodePath;
        }
        GUIUtility.systemCopyBuffer = nodePath;
    }


    [MenuItem("GameObject/隐藏节点", priority = 14)]
    public static void HideObjectNodes()
    {
        GameObject node = Selection.activeGameObject;
        if (node == null)
        {
            Debug.LogError("请选择模型节  点!");
            return;
        }
        string nodePath = node.name;
        Transform[] childs = node.GetComponentsInChildren<Transform>(true);      
        for (int i = 0; i < childs.Length; i++)
        {
            if (!childs[i].gameObject.activeInHierarchy)
            {
                Renderer rd=childs[i].GetComponent<Renderer>();
                if (rd!=null)
                {
                    rd.enabled = false;
                    Debug.Log("隐藏的物体节点： " + rd.name);
                }                                       

            
            }
        }        
    }
}
#endif