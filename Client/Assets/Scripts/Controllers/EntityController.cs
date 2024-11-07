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

    private EntityState state = EntityState.Idle;
    public EntityState State
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

    private Direction lastMoveDir;
    private Direction moveDir = Direction.Down;
    public Direction MoveDir
    {
        get
        {
            return moveDir;
        }
        set
        {
            if (State == EntityState.Move)
                return;

            if (moveDir == value)
                return;

            moveDir = value;
            if (value != Direction.None)
                lastMoveDir = value;

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
        SetPosition();
        Move();
    }

    void SetPosition()
    {
        if (State == EntityState.Idle && MoveDir != Direction.None)
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
                if (Manager.Object.FindEntityOnMap(destPos) == null)
                {
                    CellPos = destPos;
                }
            }

            State = EntityState.Move;
        }
    }

    void Move()
    {
        if (State != EntityState.Move)
            return;

        Vector3 destination = Manager.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f);
        Vector3 dir = destination - transform.position;

        float dist = dir.magnitude;

        if (dist < 0.1f)
        {
            transform.position = destination;

            // 수동으로 애니메이션 컨트롤
            state = EntityState.Idle;
            if (MoveDir == Direction.None)
                UpdateAnimation();
        }
        else
        {
            transform.position += dir.normalized * speed * Time.deltaTime;
            State = EntityState.Move;
        }
    }
}
