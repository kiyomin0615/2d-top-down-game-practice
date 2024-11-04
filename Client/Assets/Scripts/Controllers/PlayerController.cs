using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 10.0f;

    Vector3 destPosition;

    enum PlayerState
    {
        Idle,
        Moving,
        Dead
    }

    PlayerState state = PlayerState.Idle;

    void Start()
    {
        // subscribe
        Manager.Input.MouseAction -= OnMouseInput;
        Manager.Input.MouseAction += OnMouseInput;
    }

    void Update()
    {
        // state pattern
        switch (state)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Dead:
                UpdateDead();
                break;
        }
    }

    void UpdateIdle()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetFloat("speed", 0.0f);
    }

    void UpdateMoving()
    {
        Vector3 dir = destPosition - transform.position;

        if (dir.magnitude < 0.00001f)
        {
            state = PlayerState.Idle;
        }
        else
        {
            float moveDistance = Mathf.Clamp(speed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDistance;
            // transform.LookAt(destPosition);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }

        Animator animator = GetComponent<Animator>();
        animator.SetFloat("speed", speed);
    }

    void UpdateDead() { }

    void OnMouseInput(Definition.MouseEvent mouseEvent)
    {
        RaycastHit hitInfo;
        int layerMask = LayerMask.GetMask("Wall");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
        if (Physics.Raycast(ray, out hitInfo, 100.0f, layerMask))
        {
            destPosition = hitInfo.point;
            state = PlayerState.Moving;
        }
    }
}
