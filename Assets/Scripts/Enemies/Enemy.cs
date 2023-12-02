using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 0f;

    private Animator animator;

    private void Start()
    {
        currentHealth = maxHealth;
        player = FindObjectOfType<PlayerActions>().transform;
        animator = GetComponent<Animator>();
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
        Debug.Log(gameObject.name + " HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
