using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf.Protocol;

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
        if (Dir != Direction.None)
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
            Dir = Direction.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Dir = Direction.Down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Dir = Direction.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Dir = Direction.Right;
        }
        else
        {
            Dir = Direction.None;
        }
    }

    public override void MoveToNextPos()
    {
        // To Idle State
        if (Dir == Direction.None)
        {
            State = EntityState.Idle;
            SendMovePacketIfUpdated();
            return;
        }

        Vector3Int destPos = CellPos;

        switch (Dir)
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
            if (Manager.Object.FindEntityOnMap(destPos) == null)
            {
                CellPos = destPos;
            }
        }

        SendMovePacketIfUpdated();
    }

    void SendMovePacketIfUpdated()
    {
        if (isUpdated)
        {
            C_Move movePacket = new C_Move();
            movePacket.PositionInfo = PositionInfo;
            Manager.Network.Send(movePacket);
            isUpdated = false;
        }
    }
}
