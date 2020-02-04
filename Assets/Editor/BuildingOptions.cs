using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System;
using System.IO;

public class BuildingOptions : MonoBehaviour
{
    [MenuItem("CustomBuild/Build headless (Win64)")]
    static void BuildHeadless()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.options = BuildOptions.EnableHeadlessMode;
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        string path = Application.dataPath + "/../../GameBuilds/SlowSpaceGame/headless_server/server_windows_64.exe";
        string p2 = Path.GetFullPath((new Uri(path)).LocalPath);
        buildPlayerOptions.locationPathName = p2;
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/HeadlessServer.unity" };

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize / (1024 ^ 2) + " Mbs");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
}
