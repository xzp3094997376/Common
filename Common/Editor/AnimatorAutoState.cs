using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class AnimatorAutoState : EditorWindow
{
    public Object go;
    static GUIStyle style;
    [MenuItem("GTATool/自动生成动画切片状态")]
  
    static void Init()
    {
        AnimatorAutoState wind = EditorWindow.GetWindow<AnimatorAutoState>("动画状态机自动生成", true);
        wind.Show();
        style = new GUIStyle();
        style.fontStyle = FontStyle.Italic;
        style.normal.textColor = Color.green;
    }
    private void OnGUI()
    {      
        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField("请选择需要制作动画化状态机的模型", style);
        go = EditorGUILayout.ObjectField(go,typeof(Object),false);
        if(GUILayout.Button("开始制作动画状态机"))
        {
            if (go != null)
            {
                DoCreateAnimationAssets(go);
            }
        }
        
    }

    static void DoCreateAnimationAssets(Object ob)
    {
        string path = AssetDatabase.GetAssetPath(ob);
        int index = path.LastIndexOf("/");
        string path2 = path.Substring(0, index + 1);
        Debug.Log(path2);
        //创建animationController文件，保存在Assets路径下
        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(path2 +"工厂动画"+ ".controller");
        //得到它的Layer， 默认layer为base 你可以去拓展
        AnimatorControllerLayer layer = animatorController.layers[0];
        AnimatorStateMachine sm = layer.stateMachine;
        AddStateTransition(path, layer);
    }

    private static void AddStateTransition(string path, AnimatorControllerLayer layer)
    {
        AnimatorStateMachine sm = layer.stateMachine;
        //根据动画文件读取它的AnimationClip对象
        var datas = AssetDatabase.LoadAllAssetsAtPath(path);
       //AnimatorState idle = sm.AddState("AniState");
        for (int i=0;i< datas.Length;i++)
        {
            Object data = datas[i];
            if (!(data is AnimationClip))
            {
                continue;
            }
            // AnimatorState _emptyState = sm.AddState("Empty");
            AnimationClip newClip = data as AnimationClip;
            //取出动画名子 添加到state里面
            AnimatorState state = sm.AddState(newClip.name);
            Debug.Log(newClip.name);
            state.motion = newClip;
            state.writeDefaultValues = false;
            //把state添加在layer里面
            // AnimatorStateTransition trans = sm.AddAnyStateTransition(state);
            //把默认的时间条件删除
            //trans.hasExitTime = false;
        }  
    }
}
