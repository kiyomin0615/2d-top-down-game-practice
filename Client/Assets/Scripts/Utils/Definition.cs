using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Definition
{
    public enum CameraMode
    {
        QuaterView,
    }

    public enum MouseEvent
    {
        Click,
        Press,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum SceneType
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }

    public enum AudioType
    {
        BGM,
        SFX,
    }
}
