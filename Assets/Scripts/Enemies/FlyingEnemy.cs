using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float damage = 12f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 0f;
    [SerializeField] private float meleeRange = 2.5f;
    [SerializeField] private float attackRange = 9f;
    [SerializeField] private float stopDistance = 7f;
    [SerializeField] private float fireRate = 5f;
    [SerializeField] private float bulletForce = 5f;
    [SerializeField] private float xpGiven;
    [SerializeField] private GameObject proyectile;
    [SerializeField] private Transform aim;
    [SerializeField] private Transform playerHitboxCenter;
    private float nextTimeToFire = 0f;
    private float distance;
    private Vector3 moveDirection;
    private bool isAttacking = false;

    private const string IS_PLAYER = "Player";
    private const string X_DIR = "xDir";
    private const string Y_DIR = "yDir";
    private const string IS_ATTACKING = "isAttacking";

    private GameObject player;
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
        distance = Vector3.Distance(transform.position, player.transform.position);

        if (!isAttacking)
        {
            if (distance > stopDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                moveDirection = (player.transform.position - transform.position).normalized;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position, moveSpeed * Time.deltaTime);
                moveDirection = (player.transform.position - transform.position).normalized;
            }
            
            SetAnimation(moveDirection);
        }

        Vector3 aimDirection = (player.transform.position - aim.transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aim.eulerAngles = new Vector3(0, 0, angle);

        if (distance <= attackRange && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

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
            playerStats.totalKills += 1;
            playerStats.GainExp(xpGiven);
            Destroy(gameObject);
        }
    }

    private void Shoot()
    {
        GameObject enemyBullet = Instantiate(proyectile, aim.transform.position, aim.rotation);

        Rigidbody2D enemyBulletRb = enemyBullet.GetComponent<Rigidbody2D>();

        enemyBulletRb.AddForce(aim.right * bulletForce, ForceMode2D.Impulse);

        Destroy(enemyBullet, 2.5f);
    }

    private IEnumerator Attack(Vector3 moveDir)
    {
        isAttacking = true;
        animator.SetFloat(X_DIR, moveDir.x);
        animator.SetFloat(Y_DIR, moveDir.y);
        animator.SetLayerWeight(1, 1);
        animator.SetBool(IS_ATTACKING, true);

        yield return new WaitForSeconds(0.24f);

        if (distance <= meleeRange)
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
