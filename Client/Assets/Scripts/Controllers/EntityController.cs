using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Definition;

public class EntityController : MonoBehaviour
{
    public float speed = 5.0f;

    public Vector3Int CellPos { get; set; } = new Vector3Int(0, -4);

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    protected EntityState state = EntityState.Idle;
    public virtual EntityState State
    {
        get { return state; }
        set
        {
            if (state == value)
                return;

            state = value;

            UpdateAnimation();
        }
    }

    protected Direction lastMoveDir = Direction.Down;
    protected Direction moveDir = Direction.Down;
    public Direction MoveDir
    {
        get
        {
            return moveDir;
        }
        set
        {
            if (moveDir == value)
                return;

            moveDir = value;
            if (value != Direction.None)
            {
                lastMoveDir = value;
            }

            UpdateAnimation();
        }
    }

    protected virtual void UpdateAnimation()
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
            // TODO
            switch (lastMoveDir)
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

        switch (lastMoveDir)
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

    public virtual void MoveToNextPos()
    {
        // To Idle State
        if (MoveDir == Direction.None)
        {
            State = EntityState.Idle;
            return;
        }

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
            if (Manager.Object.FindEntityOnMap(destPos) == null)
            {
                CellPos = destPos;
            }
        }
    }
}
