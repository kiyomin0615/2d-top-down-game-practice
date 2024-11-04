using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = path.LastIndexOf("/");
            if (index > -1)
            {
                name = path.Substring(index + 1);
            }

            GameObject go = Manager.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string name, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{name}");
        if (original == null)
        {
            Debug.Log($"Failed to load the prefab: {name}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
            return Manager.Pool.Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original);
        go.name = original.name;

        return go;
    }

    public void Destroy(GameObject target, float delay = 0f)
    {
        if (target == null)
        {
            return;
        }

        Poolable poolable = target.GetComponent<Poolable>();
        if (poolable != null)
        {
            Manager.Pool.Push(poolable);
            return;
        }

        Object.Destroy(target, delay);
    }
}
