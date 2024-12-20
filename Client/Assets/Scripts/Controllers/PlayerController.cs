using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf.Protocol;

public class PlayerController : EntityController
{
    protected bool isSimpleAttack = true;

    protected Coroutine coSkill;

    protected override void Init()
    {
        base.Init();
    }

    protected override void UpdateAnimation()
    {
        if (State == EntityState.Idle)
        {
            switch (lastDir)
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
            switch (Dir)
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
            switch (lastDir)
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

    protected override void UpdateController()
    {
        base.UpdateController();
    }

    protected override void UpdateIdleState()
    {
        // To Move State
        if (Dir != Direction.None)
        {
            State = EntityState.Move;
            return;
        }
    }

    protected virtual void SendPacketIfUpdated() { }

    public void UseSkill(int skillId)
    {
        if (skillId == 1)
        {
            coSkill = StartCoroutine("CoAttackPunch");
        }
    }

    IEnumerator CoAttackPunch()
    {
        isSimpleAttack = true;
        State = EntityState.Skill;
        yield return new WaitForSeconds(0.3f);
        State = EntityState.Idle;
        coSkill = null;
    }

    IEnumerator CoShootArrow()
    {
        GameObject arrow = Manager.Resource.Instantiate("Entity/Arrow");
        ArrowController arrowController = arrow.GetComponent<ArrowController>();
        arrowController.Dir = lastDir;
        arrowController.CellPos = CellPos;

        isSimpleAttack = false;

        yield return new WaitForSeconds(0.3f);
        State = EntityState.Idle;
        coSkill = null;
    }

    public override void OnTakeDamage()
    {
        Debug.Log("PLAYER HIT!");
    }
}
