using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ModelToCenter {
    [MenuItem("MyMenu/Do Test")]
    static void Test()
    {
        Transform parent = Selection.activeGameObject.transform;
        Vector3 postion = parent.position;
        Quaternion rotation = parent.rotation;
        Vector3 scale = parent.localScale;
        parent.position = Vector3.zero;
        parent.rotation = Quaternion.Euler(Vector3.zero);
        parent.localScale = Vector3.one;//父物体归零


        Vector3 center = Vector3.zero;
        Renderer[] renders = parent.GetComponentsInChildren<Renderer>();
        foreach (Renderer child in renders)
        {
            center += child.bounds.center;//  
        }
        center /= parent.GetComponentsInChildren<Transform>().Length;
        Bounds bounds = new Bounds(center, Vector3.zero);
        foreach (Renderer child in renders)//计算（归零的父物体下）子物体位置
        {
            bounds.Encapsulate(child.bounds);
        }

        Debug.LogError(renders.Length);


        parent.position = postion; //父物体复原位置
        parent.rotation = rotation;
        parent.localScale = scale;

        foreach (Transform t in parent)//将子物体放在复原的的父物体下面
        {
            t.position = t.position - bounds.center;//0+向量差值
        }
        parent.transform.position = bounds.center + parent.position;//center+相对位置

  

    }
}
