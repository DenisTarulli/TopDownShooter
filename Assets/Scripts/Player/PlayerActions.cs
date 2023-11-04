using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;

    private Animator anim;
    private Rigidbody2D rb;
    private Vector3 moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        anim.SetFloat("xDir", 0f);
        anim.SetFloat("yDir", 0f);
    }

    private void Update()
    {
        float xInput = 0f;
        float yInput = 0f;

        if (Input.GetKey(KeyCode.W))
            yInput = 1f;
        if (Input.GetKey(KeyCode.A))
            xInput = -1f;
        if (Input.GetKey(KeyCode.S))
            yInput = -1f;
        if (Input.GetKey(KeyCode.D))
            xInput = 1f;

        moveDirection = new Vector3(xInput, yInput, 0f).normalized;

        SetAnimation(moveDirection);
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDirection * moveSpeed;
    }

    private void SetAnimation(Vector3 moveDir)
    {
        if (moveDir != Vector3.zero)
        {
            anim.SetLayerWeight(1, 1);
            anim.SetFloat("xDir", moveDir.x);
            anim.SetFloat("yDir", moveDir.y);
        }
        else
        {
            anim.SetLayerWeight(1, 0);
        }
        
    }
}
