using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float damage = 12f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 0f;
    private const string IS_PLAYER = "Player";

    private Animator animator;
    private EnemyHealthBar healthBar;
    private PlayerStats playerStats;

    private void Start()
    {
        currentHealth = maxHealth;
        player = FindObjectOfType<PlayerActions>().transform;
        playerStats = FindObjectOfType<PlayerStats>();
        animator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();

        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        Vector3 moveDirection = (player.transform.position - transform.position).normalized;

        SetAnimation(moveDirection);
    }

    private void SetAnimation(Vector3 moveDir)
    {
        animator.SetFloat("xDir", moveDir.x);
        animator.SetFloat("yDir", moveDir.y);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(IS_PLAYER)) return;
        playerStats.TakeDamage(damage);
    }
}
