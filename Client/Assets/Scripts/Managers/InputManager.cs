using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action<Definition.MouseEvent> MouseAction = null;

    bool pressed = false;

    // check user input
    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButton(1))
        {
            MouseAction.Invoke(Definition.MouseEvent.Press);
            pressed = true;
        }
        else
        {
            if (pressed)
            {
                MouseAction.Invoke(Definition.MouseEvent.Click);
            }
            pressed = false;
        }
    }

    public void Clear()
    {
        MouseAction = null;
    }
}
