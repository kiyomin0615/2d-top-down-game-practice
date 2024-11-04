using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : UI_Scene
{
    enum GameObjects
    {
        GridPanel
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);

        foreach (Transform child in gridPanel.transform)
        {
            Manager.Resource.Destroy(child.gameObject);
        }

        for (int i = 0; i < 12; i++)
        {
            UI_Item item = Manager.UI.GenerateSubItemUI<UI_Item>(gridPanel.transform);
            item.SetItemInfo($"ITEM {i + 1}");
        }
    }
}
