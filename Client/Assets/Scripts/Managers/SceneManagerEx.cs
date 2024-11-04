using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public void LoadScene(Definition.SceneType sceneType)
    {
        Manager.Clear();

        string sceneName = System.Enum.GetName(typeof(Definition.SceneType), sceneType);
        SceneManager.LoadScene(sceneName);
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
