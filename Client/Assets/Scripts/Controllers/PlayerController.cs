using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Grid grid;

    Vector3Int cellPos = Vector3Int.zero;

    MoveDirection moveDir = MoveDirection.None;
    float speed = 5.0f;
    bool isMoving = false;

    void Start()
    {
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
            moveDir = MoveDirection.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDir = MoveDirection.Down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveDir = MoveDirection.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDir = MoveDirection.Right;
        }
        else
        {
            moveDir = MoveDirection.None;
        }
    }

    void SetPlayerPosition()
    {
        if (!isMoving)
        {
            switch (moveDir)
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
