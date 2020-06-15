using UnityEngine;
using UnityEditor;
using System.IO;

public class CommandBuild
{
    [MenuItem("GCSeries/Build")]
    static void ExportGCSeriesPackage()
    {
        string outputPath = "Builds/";
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }
        try
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
            foreach (var path in EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes))
            {
                Debug.Log(path);
            }
            buildPlayerOptions.locationPathName = outputPath + "build.exe";
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
            buildPlayerOptions.options = BuildOptions.None;
            UnityEditor.Build.Reporting.BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                throw new System.ArgumentException("F3D_UNITY_CI_COMMAND_BUILD_FAIED", "errors count = " + report.summary.totalErrors.ToString());
            }
            Debug.Log("F3D_UNITY_CI_COMMAND_BUILD_OUTPUT_PATH = " + report.summary.outputPath.Substring(0, report.summary.outputPath.LastIndexOf('/') + 1));
        }
        catch (System.Exception e)
        {
            throw e;
        }
    }
}