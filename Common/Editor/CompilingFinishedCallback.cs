
// CompilingFinishedCallback.cs
using GCSeries;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class CompilingFinishedCallback
{

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        MRSystem mRSystem = GameObject.FindObjectOfType<MRSystem>();
        mRSystem.isAutoSlant = false;
        mRSystem.ViewerScale = 5;
        //Debug.LogError("fsdfsf");
        //Debug.Log("fd");
    }
}