using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float damage = 12f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 0f;
    [SerializeField] private float attackRange = 2.5f;
    private float distance;
    private Vector3 moveDirection;
    private bool isAttacking = false;

    private const string IS_PLAYER = "Player";
    private const string X_DIR = "xDir";
    private const string Y_DIR = "yDir";
    private const string IS_ATTACKING = "isAttacking";

    private Animator animator;
    private EnemyHealthBar healthBar;
    private PlayerStats playerStats;

    private void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindWithTag(IS_PLAYER);
        playerStats = FindObjectOfType<PlayerStats>();
        animator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();

        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        if (!isAttacking)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);

            moveDirection = (player.transform.position - transform.position).normalized;

            SetAnimation(moveDirection);
        }

        distance = Vector3.Distance(transform.position, player.transform.position);
    }

    private void SetAnimation(Vector3 moveDir)
    {     
        animator.SetFloat(X_DIR, moveDir.x);
        animator.SetFloat(Y_DIR, moveDir.y);
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

    private IEnumerator Attack(Vector3 moveDir)
    {
        isAttacking = true;
        animator.SetFloat(X_DIR, moveDir.x);
        animator.SetFloat(Y_DIR, moveDir.y);
        animator.SetLayerWeight(1, 1);
        animator.SetBool(IS_ATTACKING, true);

        yield return new WaitForSeconds(0.24f);

        if (distance <= attackRange)
            playerStats.TakeDamage(damage);

        yield return new WaitForSeconds(0.36f);

        animator.SetBool(IS_ATTACKING, false);
        isAttacking = false;
        animator.SetLayerWeight(1, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(IS_PLAYER)) return;

        if (!isAttacking)
            StartCoroutine(Attack(moveDirection));
    }
}
