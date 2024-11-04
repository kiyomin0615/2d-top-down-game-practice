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

        // temp
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            list.Add(Manager.Resource.Instantiate("UnityChan"));
        }

        foreach (GameObject go in list)
        {
            Manager.Resource.Destroy(go);
        }
    }

    public override void Clear()
    {
        Debug.Log("Login Scene Clear.");
    }
}
