using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;
using static Definition;

public class EntityController : MonoBehaviour
{
    public int Id { get; set; }

    public float speed = 5.0f;

    protected bool isUpdated = false;

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    PositionInfo positionInfo = new PositionInfo();
    public PositionInfo PositionInfo
    {
        get
        {
            return positionInfo;
        }
        set
        {
            if (positionInfo.Equals(value))
                return;

            positionInfo = value;
            UpdateAnimation();
        }
    }

    public Vector3Int CellPos
    {
        get
        {
            return new Vector3Int(PositionInfo.PosX, PositionInfo.PosY, 0);
        }
        set
        {
            if (PositionInfo.PosX == value.x && PositionInfo.PosY == value.y)
                return;

            PositionInfo.PosX = value.x;
            PositionInfo.PosY = value.y;
            isUpdated = true;
        }
    }

    public virtual EntityState State
    {
        get
        {
            return PositionInfo.State;
        }
        set
        {
            if (PositionInfo.State == value)
                return;

            PositionInfo.State = value;
            isUpdated = true;
            UpdateAnimation();
        }
    }

    protected Direction lastDir = Direction.Down;
    public Direction Dir
    {
        get
        {
            return PositionInfo.Dir;
        }
        set
        {
            if (PositionInfo.Dir == value)
                return;

            PositionInfo.Dir = value;
            if (value != Direction.None)
            {
                lastDir = value;
            }
            isUpdated = true;

            UpdateAnimation();
        }
    }

    protected virtual void UpdateAnimation()
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
            // TODO
            switch (lastDir)
            {
                case Direction.Up:
                    spriteRenderer.flipX = false;
                    animator.Play("PlayerAttackBack");
                    break;
                case Direction.Down:
                    spriteRenderer.flipX = false;
                    animator.Play("PlayerAttackFront");
                    break;
                case Direction.Left:
                    spriteRenderer.flipX = true;
                    animator.Play("PlayerAttackRight");
                    break;
                case Direction.Right:
                    spriteRenderer.flipX = false;
                    animator.Play("PlayerAttackRight");
                    break;
            }
        }
        else if (State == EntityState.Die)
        {
            // TODO
        }
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        UpdateController();
    }

    protected virtual void Init()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        transform.position = Manager.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f);
    }

    protected virtual void UpdateController()
    {
        switch (State)
        {
            case EntityState.Idle:
                UpdateIdleState();
                break;
            case EntityState.Move:
                UpdateMoveState();
                break;
            case EntityState.Skill:
                UpdateSkillState();
                break;
            case EntityState.Die:
                UpdateDieState();
                break;
        }
    }

    protected virtual void UpdateIdleState() { }

    protected virtual void UpdateMoveState()
    {
        Vector3 destination = Manager.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f);
        Vector3 dir = destination - transform.position;

        float dist = dir.magnitude;

        if (dist < 0.1f)
        {
            transform.position = destination;

            MoveToNextPos();
        }
        else
        {
            transform.position += dir.normalized * speed * Time.deltaTime;
            // State = EntityState.Move;
        }
    }

    protected virtual void UpdateSkillState()
    {

    }

    protected virtual void UpdateDieState()
    {

    }

    public Vector3Int GetForwardCellPos()
    {
        Vector3Int cellPos = CellPos;

        switch (lastDir)
        {
            case Direction.Up:
                cellPos += Vector3Int.up;
                break;
            case Direction.Down:
                cellPos += Vector3Int.down;
                break;
            case Direction.Left:
                cellPos += Vector3Int.left;
                break;
            case Direction.Right:
                cellPos += Vector3Int.right;
                break;
        }

        return cellPos;
    }

    public Direction GetDirectionOfTargetCell(Vector3Int targetCellDir)
    {
        if (targetCellDir.x > 0)
            return Direction.Right;
        else if (targetCellDir.x < 0)
            return Direction.Left;
        else if (targetCellDir.y > 0)
            return Direction.Up;
        else if (targetCellDir.y < 0)
            return Direction.Down;
        else
            return Direction.None;
    }

    public virtual void OnTakeDamage() { }

    public virtual void MoveToNextPos() { }
}
