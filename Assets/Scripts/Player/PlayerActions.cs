using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject spawner;
    [SerializeField] private PauseMenu pauseMenu;

    [Header("Variables")]
    public float moveSpeed = 7f;

    // Private references & variables
    private Animator anim;
    private GameManager gameManager;
    private Rigidbody2D rb;
    private Vector3 moveDirection;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        anim.SetFloat("xDir", 0f);
        anim.SetFloat("yDir", 0f);
    }

    private void Update()
    {
        if (!pauseMenu.gameIsPaused && gameManager.gameStarted)
        {
            float xInput = Input.GetAxisRaw("Horizontal");
            float yInput = Input.GetAxisRaw("Vertical");

            moveDirection = new Vector3(xInput, yInput, 0f).normalized;
        }

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
