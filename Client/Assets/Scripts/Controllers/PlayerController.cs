using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Grid grid;
    Vector3Int cellPos = Vector3Int.zero;

    float speed = 5.0f;

    Animator animator;
    private MoveDirection moveDir = MoveDirection.Down;
    public MoveDirection MoveDir
    {
        get
        {
            return moveDir;
        }
        set
        {
            if (isMoving)
                return;

            if (moveDir == value)
                return;

            switch (value)
            {
                case MoveDirection.Up:
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    animator.Play("PlayerMoveBack");
                    break;
                case MoveDirection.Down:
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    animator.Play("PlayerMoveFront");
                    break;
                case MoveDirection.Left:
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    animator.Play("PlayerMoveRight");
                    break;
                case MoveDirection.Right:
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    animator.Play("PlayerMoveRight");
                    break;
                case MoveDirection.None:
                    if (moveDir == MoveDirection.Up)
                    {
                        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        animator.Play("PlayerIdleBack");
                    }
                    else if (moveDir == MoveDirection.Down)
                    {
                        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        animator.Play("PlayerIdleFront");
                    }
                    else if (moveDir == MoveDirection.Left)
                    {
                        transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                        animator.Play("PlayerIdleRight");
                    }
                    else if (moveDir == MoveDirection.Right)
                    {
                        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        animator.Play("PlayerIdleRight");
                    }
                    else
                    {
                        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        animator.Play("PlayerIdleFront");
                    }
                    break;
            }

            moveDir = value;
        }
    }

    bool isMoving = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        transform.position = grid.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f);
    }

    void Update()
    {
        GetUserInput();
        SetPlayerPosition();
        Move();
    }

    void GetUserInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            MoveDir = MoveDirection.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            MoveDir = MoveDirection.Down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            MoveDir = MoveDirection.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            MoveDir = MoveDirection.Right;
        }
        else
        {
            MoveDir = MoveDirection.None;
        }
    }

    void SetPlayerPosition()
    {
        if (!isMoving)
        {
            switch (MoveDir)
            {
                case MoveDirection.Up:
                    cellPos += Vector3Int.up;
                    isMoving = true;
                    break;
                case MoveDirection.Down:
                    cellPos += Vector3Int.down;
                    isMoving = true;
                    break;
                case MoveDirection.Left:
                    cellPos += Vector3Int.left;
                    isMoving = true;
                    break;
                case MoveDirection.Right:
                    cellPos += Vector3Int.right;
                    isMoving = true;
                    break;
            }
        }
    }

    void Move()
    {
        if (!isMoving)
            return;

        Vector3 destination = grid.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f);
        Vector3 dir = destination - transform.position;

        float dist = dir.magnitude;

        if (dist < 0.1f)
        {
            transform.position = destination;
            isMoving = false;
        }
        else
        {
            transform.position += dir.normalized * speed * Time.deltaTime;
            isMoving = true;
        }
    }
}
