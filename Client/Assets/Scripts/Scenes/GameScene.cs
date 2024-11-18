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

        Manager.Map.LoadMap(1);

        GameObject player = Manager.Resource.Instantiate("Entity/Player");
        player.name = "Player";
        Manager.Object.Add(player);

        for (int i = 0; i < 5; i++)
        {
            GameObject monster = Manager.Resource.Instantiate("Entity/Monster");
            monster.name = "Monster" + i;
            Manager.Object.Add(monster);

            Vector3Int randomCellPos = new Vector3Int(Random.Range(-10, 10), Random.Range(-5, 5));
            MonsterController monsterController = monster.GetComponent<MonsterController>();
            monsterController.CellPos = randomCellPos;
        }
    }

    public override void Clear()
    {
        Debug.Log("Game Scene Clear.");
    }
}
