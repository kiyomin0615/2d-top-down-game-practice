using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    Vector3Int cellPos = new Vector3Int(0, -4);

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
        transform.position = Manager.Map.CurrentGrid.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f);
    }

    void Update()
    {
        GetUserInput();
        SetPlayerPosition();
        Move();
    }

    void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
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
        if (!isMoving && MoveDir != MoveDirection.None)
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
                isMoving = true;
            }
        }
    }

    void Move()
    {
        if (!isMoving)
            return;

        Vector3 destination = Manager.Map.CurrentGrid.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f);
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
