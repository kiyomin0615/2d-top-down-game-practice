using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    public static T FindChild<T>(GameObject root, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (root == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < root.transform.childCount; i++)
            {
                Transform child = root.transform.GetChild(i);
                if (String.IsNullOrEmpty(name) || child.name == name)
                {
                    T component = child.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in root.GetComponentsInChildren<T>())
            {
                if (String.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static GameObject FindChildGameObject(GameObject root, string name = null, bool recursive = false)
    {
        Transform childTransform = FindChild<Transform>(root, name, recursive);
        if (childTransform == null)
            return null;

        return childTransform.gameObject;
    }
}
