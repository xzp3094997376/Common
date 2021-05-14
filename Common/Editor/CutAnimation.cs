using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class CutAnimation:AssetPostprocessor
{
     string path = Application.streamingAssetsPath+"/ani.txt";  
    public void OnPreprocessModel()
    {
        Debug.Log(assetImporter.name);
//#if !CUTANI
//        Debug.Log("没有定义 CUTANI");
//        return;
//#endif
        //Bean.InitPublicExcelTables();
        ModelImporter _modelImporter = (ModelImporter)assetImporter;

        _modelImporter.animationType = ModelImporterAnimationType.Legacy;
        //_modelImporter.generateAnimations = ModelImporterGenerateAnimations.GenerateAnimations;

       
        string[] strs =File.ReadAllLines(path);
        Debug.Log(path + "  " + strs.Length);
        string sel= Selection.activeObject?.name;
        if (sel==null||strs[0]!=sel)
        {
            Debug.LogError("streamingAssetsPath/ani.txt 名字没有修改 :"+sel+":  "+strs[0]);
            return;
        }

        ModelImporterClipAnimation[] _animations = new ModelImporterClipAnimation[strs.Length-1];
        for (int i = 1; i < strs.Length; i++)
        {
            string[] aniStrs= strs[i].Split('|');
            _animations[i-1] = SetClipAnimation(i.ToString(),int.Parse(aniStrs[0]), int.Parse(aniStrs[1]),i==1);
        }
        _modelImporter.clipAnimations = _animations;           
    }

     ModelImporterClipAnimation SetClipAnimation(string _clipName, int _firstFrame, int _lastFrame, bool _isLoop)
    {
        ModelImporterClipAnimation _clip = new ModelImporterClipAnimation();
        _clip.name = _clipName;
        _clip.firstFrame = _firstFrame;
        _clip.lastFrame = _lastFrame;
        _clip.loop = _isLoop;
        if (_isLoop)
        {
            _clip.wrapMode = WrapMode.Loop;
        }
        else
        {
            _clip.wrapMode = WrapMode.Default;
        }
        return _clip;
    }
}

