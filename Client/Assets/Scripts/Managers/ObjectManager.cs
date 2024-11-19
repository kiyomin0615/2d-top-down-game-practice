using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class ObjectManager
{
    public MyPlayerController MyPlayerController { get; set; }
    Dictionary<int, GameObject> objectDict = new Dictionary<int, GameObject>();

    public void Add(PlayerInfo info, bool isMyPlayer = false)
    {
        if (isMyPlayer)
        {
            GameObject go = Manager.Resource.Instantiate("Entity/MyPlayer");
            go.name = info.Name;
            objectDict.Add(info.PlayerId, go);

            MyPlayerController = go.GetComponent<MyPlayerController>();
            MyPlayerController.Id = info.PlayerId;
            MyPlayerController.CellPos = new Vector3Int(info.PosX, info.PosY, 0);
        }
        else
        {
            GameObject go = Manager.Resource.Instantiate("Entity/Player");
            go.name = info.Name;
            objectDict.Add(info.PlayerId, go);

            PlayerController Player = go.GetComponent<PlayerController>();
            Player.Id = info.PlayerId;
            Player.CellPos = new Vector3Int(info.PosX, info.PosY, 0);
        }
    }

    public void Add(int id, GameObject go)
    {
        objectDict.Add(id, go);
    }

    public void Remove(int id)
    {
        objectDict.Remove(id);
    }

    public void RemoveMyPlayer()
    {
        if (MyPlayerController == null)
            return;

        Remove(MyPlayerController.Id);
        MyPlayerController = null;
    }

    public GameObject FindEntityOnMap(Vector3Int cellPos)
    {
        foreach (GameObject go in objectDict.Values)
        {
            EntityController entity = go.GetComponent<EntityController>();
            if (entity == null)
                continue;

            if (entity.CellPos == cellPos)
                return go;
        }

        return null;
    }

    public GameObject FindEntityOnMap(Func<GameObject, bool> func)
    {
        foreach (GameObject go in objectDict.Values)
        {
            if (func.Invoke(go))
                return go;
        }

        return null;
    }

    public void Clear()
    {
        objectDict.Clear();
    }
}
