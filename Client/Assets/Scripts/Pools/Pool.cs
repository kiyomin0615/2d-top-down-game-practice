using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    public GameObject Original { get; private set; } // prefab object
    public GameObject Root { get; private set; } // root object

    Stack<Poolable> poolableStack = new Stack<Poolable>(); // poolables

    public void Init(GameObject original, int poolSize = 5)
    {
        Original = original;
        Root = new GameObject() { name = $"@{Original.name}Root" };

        for (int i = 0; i < poolSize; i++)
        {
            Push(CreatePoolable());
        }
    }

    Poolable CreatePoolable()
    {
        GameObject go = Object.Instantiate<GameObject>(Original);
        go.name = Original.name;

        return Utility.GetOrAddComponent<Poolable>(go);
    }

    public void Push(Poolable poolable)
    {
        if (poolable == null)
            return;

        poolable.transform.parent = Root.transform;
        poolable.gameObject.SetActive(false);
        poolable.isUsing = false;

        poolableStack.Push(poolable);
    }

    public Poolable Pop(Transform parent = null)
    {
        Poolable poolable;

        if (poolableStack.Count > 0)
            poolable = poolableStack.Pop();
        else
            poolable = CreatePoolable();

        // DontDestroyOnLoad를 막기 위한 트릭
        if (parent == null)
            poolable.transform.parent = Manager.Scene.CurrentScene.transform;

        poolable.transform.parent = parent;
        poolable.gameObject.SetActive(true);
        poolable.isUsing = true;

        return poolable;
    }
}
