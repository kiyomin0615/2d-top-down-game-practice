using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    public GameObject PoolRoot { get; private set; }

    Dictionary<string, Pool> poolDict = new Dictionary<string, Pool>();

    public void Init()
    {
        if (PoolRoot == null)
        {
            PoolRoot = new GameObject() { name = "@PoolRoot" };
            Object.DontDestroyOnLoad(PoolRoot);
        }
    }

    public GameObject GetOriginal(string name)
    {
        if (poolDict.ContainsKey(name) == false)
            return null;

        return poolDict[name].Original;
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;

        if (poolDict.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        poolDict[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (poolDict.ContainsKey(original.name) == false)
            CreatePool(original);

        Pool pool = poolDict[original.name];
        Poolable poolable = pool.Pop(parent);

        return poolable;
    }

    public void CreatePool(GameObject original, int poolSize = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, poolSize);

        pool.Root.transform.parent = PoolRoot.transform;

        poolDict.Add(original.name, pool);
    }

    public void Clear()
    {
        foreach (Transform child in PoolRoot.transform)
            GameObject.Destroy(child.gameObject);

        poolDict.Clear();
    }
}
