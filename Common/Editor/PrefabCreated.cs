using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class PrefabCreated {

    static PrefabCreated()
    {
        UnityEditor.PrefabUtility.prefabInstanceUpdated += OnPrefabInstanceUpdate;
    }

    //预制体创建和apply时会调用
    static void OnPrefabInstanceUpdate(GameObject instance)
    {
        UnityEngine.Debug.Log("[Callback] Prefab.Apply on instance named :" + instance.name);

        GameObject prefab = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(instance) as GameObject;
        string prefabPath = AssetDatabase.GetAssetPath(prefab);
        UnityEngine.Debug.Log("@Prefab originPath=" + prefabPath);

        ModelControl mc = prefab.GetComponent<ModelControl>();
        if (mc != null)
        {
            Debug.LogError("已经挂载过脚本：  " + prefabPath);
            return;
        }


        Transform child= prefab.transform.GetChild(0);
        child.localPosition = Vector3.zero;

      

        ModelControl control= prefab.AddComponent<ModelControl>();

        return;

        ModelAdditionalManager mgr= prefab.AddComponent<ModelAdditionalManager>();
        mgr.operationTypes = new OperationType[] {OperationType.Theory,OperationType.Disassemble,OperationType.Explode };
        mgr.buttonSpacing = 80;
        mgr.leftPadding = 220;
       
        //原理
        ModelTheoryAnimPlay mtp= prefab.AddComponent<ModelTheoryAnimPlay>();
        mtp.aniName = "1";
        mtp.modelAni = child.GetComponent<Animation>();

        ModelDisassembleOperation mdo = prefab.AddComponent<ModelDisassembleOperation>();
        mdo.animName = "1";
        mdo.model = child.GetComponent<Animation>();
        mdo.installMode = InstallOperationType.Animation;
        Animation amt= child.GetComponent<Animation>();
        AnimationClip[] clips = new AnimationClip[amt.GetClipCount() - 2];
        for (int i = 3,j=0; j < clips.Length; i++,j++)
        {
            clips[j]= amt.GetClip(i.ToString());
        }
        mdo.installClips = clips;

        //原理
        ModelExplodePlay mep = prefab.AddComponent<ModelExplodePlay>();
        mep.ExploreName = "2";
        mep.model = child.GetComponent<Animation>();
        mep.IdelName = "1";



    }
}

