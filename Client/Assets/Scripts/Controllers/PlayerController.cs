using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Definition;

public class PlayerController : EntityController
{
    protected override void Init()
    {
        base.Init();
    }

    protected override void UpdateController()
    {
        GetUserInput();
        base.UpdateController();
    }

    void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    void GetUserInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            MoveDir = MoveDirection.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            MoveDir = MoveDirection.Down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            MoveDir = MoveDirection.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            MoveDir = MoveDirection.Right;
        }
        else
        {
            MoveDir = MoveDirection.None;
        }
    }
}
