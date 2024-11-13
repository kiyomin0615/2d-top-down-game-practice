using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Definition;

public class MonsterController : EntityController
{
    Vector3Int destCellPos;

    Coroutine coPatrol;

    Coroutine coSearchPlayer;
    GameObject target;
    [SerializeField] float searchRange = 20.0f;

    [SerializeField] bool isSimpleAttack = true;
    Coroutine coSkill;
    [SerializeField] float skillRange = 1.0f;


    public override EntityState State
    {
        get { return state; }
        set
        {
            if (state == value)
                return;

            base.State = value;

            if (coPatrol != null)
            {
                StopCoroutine(coPatrol);
                coPatrol = null;
            }

            if (coSearchPlayer != null)
            {
                StopCoroutine(coSearchPlayer);
                coSearchPlayer = null;
            }
        }
    }

    protected override void Init()
    {
        base.Init();

        State = EntityState.Idle;
        MoveDir = Direction.None;
        speed = 3.0f;
        isSimpleAttack = UnityEngine.Random.Range(0f, 1f) < 0.5f ? true : false;
        if (isSimpleAttack)
        {
            skillRange = 1.0f;
        }
        else
        {
            skillRange = 20.0f;
        }
    }

    protected override void UpdateIdleState()
    {
        base.UpdateIdleState();

        // Patrol
        if (coPatrol == null)
        {
            coPatrol = StartCoroutine("CoPatrol");
        }

        // Search Player
        if (coSearchPlayer == null)
        {
            coSearchPlayer = StartCoroutine("CoSearchPlayer");
        }
    }

    IEnumerator CoPatrol()
    {
        int duration = UnityEngine.Random.Range(1, 4);
        yield return new WaitForSeconds(duration);

        // try 10 times
        for (int i = 0; i < 10; i++)
        {
            int randomX = UnityEngine.Random.Range(-16, 16);
            int randomY = UnityEngine.Random.Range(-9, 13);
            Vector3Int randomPos = CellPos + new Vector3Int(randomX, randomY, 0);

            if (Manager.Map.CanGo(randomPos) && Manager.Object.FindEntityOnMap(randomPos) == null)
            {
                destCellPos = randomPos;
                State = EntityState.Move;
                yield break; // return
            }
        }

        State = EntityState.Idle;
    }

    IEnumerator CoSearchPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (target != null)
                continue;

            target = Manager.Object.FindEntityOnMap((GameObject go) =>
            {
                PlayerController playerController = go.GetComponent<PlayerController>();
                if (playerController == null)
                    return false;

                Vector3Int dir = playerController.CellPos - CellPos;
                if (dir.magnitude > searchRange)
                    return false;

                return true;
            });
        }
    }

    public override void MoveToNextPos()
    {
        Vector3Int destCellPos = this.destCellPos;

        if (target != null)
        {
            destCellPos = target.GetComponent<PlayerController>().CellPos;

            Vector3Int targetDir = destCellPos - CellPos;
            if (targetDir.magnitude <= skillRange && (targetDir.x == 0 || targetDir.y == 0))
            {
                MoveDir = GetDirectionOfTargetCell(targetDir);
                State = EntityState.Skill;

                if (isSimpleAttack)
                {
                    coSkill = StartCoroutine("CoAttackPunch");
                }
                else
                {
                    coSkill = StartCoroutine("CoShootArrow");
                }
                return;
            }
        }

        List<Vector3Int> path = Manager.Map.FindPath(CellPos, destCellPos, ignoreDestCellCollision: true);
        if (path.Count < 2 || (target != null && path.Count > searchRange))
        {
            target = null;
            State = EntityState.Idle;
            return;
        }

        Vector3Int nextCellPos = path[1];
        Vector3Int dir = nextCellPos - CellPos;

        MoveDir = GetDirectionOfTargetCell(dir);

        if (Manager.Map.CanGo(nextCellPos) && Manager.Object.FindEntityOnMap(nextCellPos) == null)
        {
            CellPos = nextCellPos;
        }
        else
        {
            // To Idle State
            State = EntityState.Idle;
        }
    }

    IEnumerator CoAttackPunch()
    {
        GameObject target = Manager.Object.FindEntityOnMap(GetForwardCellPos());
        if (target != null)
        {
            EntityController controller = target.GetComponent<EntityController>();
            if (controller != null)
                controller.OnTakeDamage();
        }

        isSimpleAttack = true;

        yield return new WaitForSeconds(0.3f);
        State = EntityState.Move;
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
        State = EntityState.Move;
        coSkill = null;
    }

    public override void OnTakeDamage()
    {
        GameObject dieEffect = Manager.Resource.Instantiate("Effect/DieEffect");
        dieEffect.transform.position = gameObject.transform.position;
        dieEffect.GetComponent<Animator>().Play("DieEffect");
        Manager.Resource.Destroy(dieEffect, 0.5f);

        Manager.Object.Remove(gameObject);
        Manager.Resource.Destroy(gameObject);
    }
}
