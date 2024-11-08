using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Definition;

public class ArrowController : EntityController
{
    protected override void Init()
    {
        base.Init();

        switch (MoveDir)
        {
            case Direction.Up:
                transform.rotation = UnityEngine.Quaternion.Euler(0f, 0f, 0f);
                break;
            case Direction.Down:
                transform.rotation = UnityEngine.Quaternion.Euler(0f, 0f, 180f);
                break;
            case Direction.Left:
                transform.rotation = UnityEngine.Quaternion.Euler(0f, 0f, 90f);
                break;
            case Direction.Right:
                transform.rotation = UnityEngine.Quaternion.Euler(0f, 0f, -90f);
                break;
        }
    }

    protected override void UpdateAnimation() { }

    protected override void UpdateIdleState()
    {
        // Move
        if (MoveDir != Direction.None)
        {
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

            if (Manager.Map.CanGo(destPos))
            {
                GameObject enemy = Manager.Object.FindEntityOnMap(destPos);
                if (enemy == null)
                {
                    CellPos = destPos;
                }
                else
                {
                    GameObject dieEffect = Manager.Resource.Instantiate("Effect/DieEffect");
                    dieEffect.transform.position = enemy.transform.position;
                    dieEffect.GetComponent<Animator>().Play("DieEffect");
                    Manager.Resource.Destroy(dieEffect, 0.5f);

                    Manager.Object.Remove(enemy);
                    Manager.Resource.Destroy(enemy);

                    Manager.Resource.Destroy(gameObject);
                }
            }
            else
            {
                Manager.Resource.Destroy(gameObject);
            }

            State = EntityState.Move;
        }
    }
}
