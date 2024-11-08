using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Definition;

public class MonsterController : EntityController
{
    Coroutine coPatrol;
    Vector3Int destCellPos;

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
        }
    }

    protected override void Init()
    {
        base.Init();

        State = EntityState.Idle;
        MoveDir = Direction.None;
    }

    protected override void UpdateIdleState()
    {
        base.UpdateIdleState();

        // Patrol
        if (coPatrol == null)
        {
            coPatrol = StartCoroutine("CoPatrol");
        }
    }

    IEnumerator CoPatrol()
    {
        int duration = UnityEngine.Random.Range(1, 5);
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

    public override void MoveToNextPos()
    {
        // TODO : path-finding algorithm
        Vector3Int dir = destCellPos - CellPos;

        if (dir.x > 0)
            MoveDir = Direction.Right;
        else if (dir.x < 0)
            MoveDir = Direction.Left;
        else if (dir.y > 0)
            MoveDir = Direction.Up;
        else if (dir.y < 0)
            MoveDir = Direction.Down;
        else
            MoveDir = Direction.None;

        Vector3Int destPos = CellPos;

        switch (MoveDir)
        {
            case Direction.Up:
                destPos += Vector3Int.up;
                break;
            case Direction.Down:
                destPos += Vector3Int.down;
                break;
            case Direction.Left:
                destPos += Vector3Int.left;
                break;
            case Direction.Right:
                destPos += Vector3Int.right;
                break;
        }

        if (Manager.Map.CanGo(destPos) && Manager.Object.FindEntityOnMap(destPos) == null)
        {
            CellPos = destPos;
        }
        else
        {
            // To Idle State
            State = EntityState.Idle;
        }
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
