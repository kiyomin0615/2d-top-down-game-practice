using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Definition;

public class EntityController : MonoBehaviour
{
    public float speed = 5.0f;

    protected Vector3Int cellPos = new Vector3Int(0, -4);

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
        }
    }

    private MoveDirection lastMoveDir;
    private MoveDirection moveDir = MoveDirection.Down;
    public MoveDirection MoveDir
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

            switch (value)
            {
                case MoveDirection.Up:
                    spriteRenderer.flipX = false;
                    animator.Play("PlayerMoveBack");
                    break;
                case MoveDirection.Down:
                    spriteRenderer.flipX = false;
                    animator.Play("PlayerMoveFront");
                    break;
                case MoveDirection.Left:
                    spriteRenderer.flipX = true;
                    animator.Play("PlayerMoveRight");
                    break;
                case MoveDirection.Right:
                    spriteRenderer.flipX = false;
                    animator.Play("PlayerMoveRight");
                    break;
            }

            moveDir = value;
            if (value != MoveDirection.None)
                lastMoveDir = value;
        }
    }

    protected virtual void UpdateAnimation()
    {
        if (state == EntityState.Idle)
        {
            switch (lastMoveDir)
            {
                case MoveDirection.Up:
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    animator.Play("PlayerIdleBack");
                    break;
                case MoveDirection.Down:
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    animator.Play("PlayerIdleFront");
                    break;
                case MoveDirection.Right:
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    animator.Play("PlayerIdleRight");
                    break;
                case MoveDirection.Left:
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    animator.Play("PlayerIdleFront");
                    break;
            }
        }
        else if (state == EntityState.Move)
        {

        }
        else if (state == EntityState.Skill)
        {

        }
        else if (state == EntityState.Die)
        {

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
        transform.position = Manager.Map.CurrentGrid.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f);
    }

    protected virtual void UpdateController()
    {
        SetPosition();
        Move();
    }

    void SetPosition()
    {
        if (State == EntityState.Idle && MoveDir != MoveDirection.None)
        {
            Vector3Int destPos = cellPos;

            switch (MoveDir)
            {
                case MoveDirection.Up:
                    destPos += Vector3Int.up;
                    break;
                case MoveDirection.Down:
                    destPos += Vector3Int.down;
                    break;
                case MoveDirection.Left:
                    destPos += Vector3Int.left;
                    break;
                case MoveDirection.Right:
                    destPos += Vector3Int.right;
                    break;
            }

            if (Manager.Map.CanGo(destPos))
            {
                cellPos = destPos;
                State = EntityState.Move;
            }
        }
    }

    void Move()
    {
        if (State != EntityState.Move)
            return;

        Vector3 destination = Manager.Map.CurrentGrid.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f);
        Vector3 dir = destination - transform.position;

        float dist = dir.magnitude;

        if (dist < 0.1f)
        {
            transform.position = destination;
            State = EntityState.Idle;
        }
        else
        {
            transform.position += dir.normalized * speed * Time.deltaTime;
            State = EntityState.Move;
        }
    }
}
