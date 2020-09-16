using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterialRenderQueue : MonoBehaviour
{
	//让模型永远显示在前面
    public int RenderQueue = 6000;
    private void Awake()
    {
        StartCoroutine(SetMaterials());
    }

    private IEnumerator SetMaterials()
    {
        MeshRenderer[] meshRenderers = transform.GetComponentsInChildren<MeshRenderer>(true);
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            for (int j = 0; j < meshRenderers[i].materials.Length; j++)
            {
                meshRenderers[i].materials[j].renderQueue = RenderQueue;
            }
        }
        SkinnedMeshRenderer[] skinnedMeshRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        for (int i = 0; i < skinnedMeshRenderers.Length; i++)
        {
            for (int j = 0; j < skinnedMeshRenderers[i].materials.Length; j++)
            {
                skinnedMeshRenderers[i].materials[j].renderQueue = RenderQueue;
                //yield return null;
            }
        }
        yield return null;
    }
}
