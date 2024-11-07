using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Definition;

public class MonsterController : EntityController
{
    protected override void Init()
    {
        base.Init();

        State = EntityState.Idle;
        MoveDir = Direction.None;
    }

    protected override void UpdateController()
    {
        // GetUserInput();
        base.UpdateController();
    }

    void GetUserInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            MoveDir = Direction.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            MoveDir = Direction.Down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            MoveDir = Direction.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            MoveDir = Direction.Right;
        }
        else
        {
            MoveDir = Direction.None;
        }
    }
}
