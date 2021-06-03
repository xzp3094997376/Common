using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class MyTexturePostprocessor : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        if (EditorUserBuildSettings.activeBuildTarget==BuildTarget.Android|| EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
        {
            //Debug.Log(assetPath +"dsds");        
            if (assetPath.Contains("map"))
            {
                Debug.Log("模型贴图导入");
                TextureImporter textureImporter = (TextureImporter)assetImporter;
                if (textureImporter == null)
                {
                    Debug.Log("不是图片");
                    return;
                }
                textureImporter.maxTextureSize = 512;
            }
        }
      
    }

    
}
