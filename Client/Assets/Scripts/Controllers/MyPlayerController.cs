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
        if (coSkillCooltime == null && Input.GetKey(KeyCode.F))
        {
            Debug.Log("PUNCH!");

            C_Skill skillPacket = new C_Skill() { SkillInfo = new SkillInfo() };
            skillPacket.SkillInfo.SkillId = 1;
            Manager.Network.Send(skillPacket);

            coSkillCooltime = StartCoroutine("CoWaitForCooltime", 0.3f);
        }
        else if (coSkillCooltime == null && Input.GetMouseButton(0))
        {
            Debug.Log("SHOOT ARROW!");

            C_Skill skillPacket = new C_Skill() { SkillInfo = new SkillInfo() };
            skillPacket.SkillInfo.SkillId = 2;
            Manager.Network.Send(skillPacket);

            coSkillCooltime = StartCoroutine("CoWaitForCooltime", 0.3f);
        }
    }

    Coroutine coSkillCooltime;
    IEnumerator CoWaitForCooltime(float cooltime)
    {
        yield return new WaitForSeconds(cooltime);
        coSkillCooltime = null;
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
            SendPacketIfUpdated();
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

        SendPacketIfUpdated();
    }

    protected override void SendPacketIfUpdated()
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
