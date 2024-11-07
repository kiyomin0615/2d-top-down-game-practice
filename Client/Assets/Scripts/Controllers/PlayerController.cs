using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Definition;

public class PlayerController : EntityController
{
    Coroutine coSkill;

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
                GetSkillInput();
                break;
            case EntityState.Move:
                GetMoveInput();
                break;
        }

        base.UpdateController();
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

    void GetSkillInput()
    {
        if (Input.GetKey(KeyCode.F))
        {
            State = EntityState.Skill;
            coSkill = StartCoroutine("CoAttackPunch");
        }
    }

    IEnumerator CoAttackPunch()
    {
        GameObject target = Manager.Object.FindEntityOnMap(GetForwardCellPos());
        if (target != null)
        {
            // TODO
            Debug.Log($"{target.name} got damage.");
        }

        yield return new WaitForSeconds(0.5f);
        State = EntityState.Idle;
        coSkill = null;
    }
}
