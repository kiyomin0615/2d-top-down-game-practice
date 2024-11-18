using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Definition.SceneType SceneType { get; protected set; } = Definition.SceneType.Unknown;

    void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        Screen.SetResolution(640, 480, false);

        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            Manager.Resource.Instantiate("UI/EventSystem");
    }

    public abstract void Clear();
}
