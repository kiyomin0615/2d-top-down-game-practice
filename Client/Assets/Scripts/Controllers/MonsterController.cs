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
        base.UpdateController();
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
