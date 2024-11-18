using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildEditor
{
    static void BuildAndRunClientsOnWin64(int playerCount)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);

        for (int i = 0; i < playerCount; i++)
        {
            BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/Win64/" + GetProjectName() + i + "/" + GetProjectName() + i + ".exe", BuildTarget.StandaloneWindows64, BuildOptions.AutoRunPlayer);
        }
    }

    [MenuItem("Tools/Build And Run/2 Clients")]
    static void BuildAndRunClientsOnWin64_2()
    {
        BuildAndRunClientsOnWin64(2);
    }
    [MenuItem("Tools/Build And Run/4 Clients")]
    static void BuildAndRunClientsOnWin64_4()
    {
        BuildAndRunClientsOnWin64(4);
    }
    [MenuItem("Tools/Build And Run/6 Clients")]
    static void BuildAndRunClientsOnWin64_6()
    {
        BuildAndRunClientsOnWin64(6);
    }

    static string GetProjectName()
    {
        string[] s = Application.dataPath.Split("/"); // Application.dataPath는 현재 Unity 프로젝트의 Assets 폴더 경로
        return s[s.Length - 2];
    }

    static string[] GetScenePaths()
    {
        string[] paths = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < paths.Length; i++)
        {
            paths[i] = EditorBuildSettings.scenes[i].path;
        }

        return paths;
    }
}
