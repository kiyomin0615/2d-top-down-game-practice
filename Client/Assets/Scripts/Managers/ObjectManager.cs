using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    List<GameObject> objects = new List<GameObject>();

    public void Add(GameObject go)
    {
        objects.Add(go);
    }

    public void Remove(GameObject go)
    {
        objects.Remove(go);
    }

    public GameObject FindEntityOnMap(Vector3Int cellPos)
    {
        foreach (GameObject go in objects)
        {
            EntityController entity = go.GetComponent<EntityController>();
            if (entity == null)
                continue;

            if (entity.CellPos == cellPos)
                return go;
        }

        return null;
    }

    public void Clear()
    {
        objects.Clear();
    }
}
