using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class ObjectManager
{
    public MyPlayerController MyPlayerController { get; set; }
    Dictionary<int, GameObject> objectDict = new Dictionary<int, GameObject>();

    public void Add(PlayerInfo playerInfo, bool isMyPlayer = false)
    {
        if (isMyPlayer)
        {
            GameObject go = Manager.Resource.Instantiate("Entity/MyPlayer");
            go.name = playerInfo.Name;
            objectDict.Add(playerInfo.PlayerId, go);

            MyPlayerController = go.GetComponent<MyPlayerController>();
            MyPlayerController.Id = playerInfo.PlayerId;
            MyPlayerController.PositionInfo = playerInfo.PositionInfo;
            MyPlayerController.SyncPosition();
        }
        else
        {
            GameObject go = Manager.Resource.Instantiate("Entity/Player");
            go.name = playerInfo.Name;
            objectDict.Add(playerInfo.PlayerId, go);

            PlayerController Player = go.GetComponent<PlayerController>();
            Player.Id = playerInfo.PlayerId;
            Player.PositionInfo = playerInfo.PositionInfo;
            Player.SyncPosition();
        }
    }

    public void Remove(int id)
    {
        GameObject go = FindEntityOnMap(id);
        if (go == null)
            return;

        objectDict.Remove(id);
        Manager.Resource.Destroy(go);
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

    public GameObject FindEntityOnMap(int entityId)
    {
        GameObject go = null;
        objectDict.TryGetValue(entityId, out go);
        return go;
    }

    public void Clear()
    {
        foreach (GameObject go in objectDict.Values)
        {
            Manager.Resource.Destroy(go);
        }

        objectDict.Clear();
    }
}
