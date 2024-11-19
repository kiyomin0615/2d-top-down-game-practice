using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    void Start()
    {
        Init();
    }

    void Update()
    {
        // temp
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Manager.Scene.LoadScene(Definition.SceneType.Game);
        }
    }

    protected override void Init()
    {
        base.Init();

        SceneType = Definition.SceneType.Login;
    }

    public override void Clear()
    {
        Debug.Log("Login Scene Clear.");
    }
}
