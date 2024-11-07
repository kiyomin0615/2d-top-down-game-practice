using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Definition;

public class PlayerController : EntityController
{
    bool isSimpleAttack = true;

    Coroutine coSkill;

    protected override void UpdateAnimation()
    {
        if (State == EntityState.Idle)
        {
            switch (lastMoveDir)
            {
                case Direction.Up:
                    spriteRenderer.flipX = false;
                    animator.Play("PlayerIdleBack");
                    break;
                case Direction.Down:
                    spriteRenderer.flipX = false;
                    animator.Play("PlayerIdleFront");
                    break;
                case Direction.Left:
                    spriteRenderer.flipX = true;
                    animator.Play("PlayerIdleRight");
                    break;
                case Direction.Right:
                    spriteRenderer.flipX = false;
                    animator.Play("PlayerIdleRight");
                    break;
            }
        }
        else if (State == EntityState.Move)
        {
            switch (MoveDir)
            {
                case Direction.Up:
                    spriteRenderer.flipX = false;
                    animator.Play("PlayerMoveBack");
                    break;
                case Direction.Down:
                    spriteRenderer.flipX = false;
                    animator.Play("PlayerMoveFront");
                    break;
                case Direction.Left:
                    spriteRenderer.flipX = true;
                    animator.Play("PlayerMoveRight");
                    break;
                case Direction.Right:
                    spriteRenderer.flipX = false;
                    animator.Play("PlayerMoveRight");
                    break;
            }
        }
        else if (State == EntityState.Skill)
        {
            switch (lastMoveDir)
            {
                case Direction.Up:
                    spriteRenderer.flipX = false;
                    animator.Play(isSimpleAttack ? "PlayerAttackBack" : "PlayerAttackBackWeapon");
                    break;
                case Direction.Down:
                    spriteRenderer.flipX = false;
                    animator.Play(isSimpleAttack ? "PlayerAttackFront" : "PlayerAttackFrontWeapon");
                    break;
                case Direction.Left:
                    spriteRenderer.flipX = true;
                    animator.Play(isSimpleAttack ? "PlayerAttackRight" : "PlayerAttackRightWeapon");
                    break;
                case Direction.Right:
                    spriteRenderer.flipX = false;
                    animator.Play(isSimpleAttack ? "PlayerAttackRight" : "PlayerAttackRightWeapon");
                    break;
            }
        }
        else if (State == EntityState.Die)
        {
            // TODO
        }
    }


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
            coSkill = StartCoroutine("CoShootArrow");
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

        isSimpleAttack = true;

        yield return new WaitForSeconds(0.3f);
        State = EntityState.Idle;
        coSkill = null;
    }

    IEnumerator CoShootArrow()
    {
        GameObject arrow = Manager.Resource.Instantiate("Entity/Arrow");
        ArrowController arrowController = arrow.GetComponent<ArrowController>();
        arrowController.MoveDir = lastMoveDir;
        arrowController.CellPos = CellPos;

        isSimpleAttack = false;

        yield return new WaitForSeconds(0.3f);
        State = EntityState.Idle;
        coSkill = null;
    }
}
