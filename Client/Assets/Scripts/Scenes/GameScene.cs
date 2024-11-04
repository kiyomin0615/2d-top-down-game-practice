using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    void Start()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();

        SceneType = Definition.SceneType.Game;
        Manager.UI.OpenSceneUI<UI_Inventory>();

        // temp
        for (int i = 0; i < 10; i++)
        {
            Manager.Resource.Instantiate("UnityChan");
        }
    }

    public override void Clear()
    {
        Debug.Log("Game Scene Clear.");
    }
}
