using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Definition;

public class MyPlayerController : PlayerController
{
    protected override void Init()
    {
        base.Init();
    }

    protected override void UpdateController()
    {
        switch (State)
        {
            case EntityState.Idle:
                GetMoveInput();
                break;
            case EntityState.Move:
                GetMoveInput();
                break;
        }

        base.UpdateController();
    }

    protected override void UpdateIdleState()
    {
        // To Move State
        if (MoveDir != Direction.None)
        {
            State = EntityState.Move;
            return;
        }

        // To Skill State
        if (Input.GetKey(KeyCode.F))
        {
            State = EntityState.Skill;
            coSkill = StartCoroutine("CoAttackPunch");
        }
        else if (Input.GetMouseButton(0))
        {
            State = EntityState.Skill;
            coSkill = StartCoroutine("CoShootArrow");
        }
    }

    void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    void GetMoveInput()
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
