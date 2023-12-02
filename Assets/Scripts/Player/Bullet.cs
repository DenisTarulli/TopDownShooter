using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject hitEffect;
    private const string IS_ENEMY = "Enemy";
    private const string IS_FLYING_ENEMY = "FlyingEnemy";
    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hitFx = Instantiate(hitEffect, transform.position, Quaternion.identity);

        if (collision.gameObject.CompareTag(IS_ENEMY))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(playerStats.bulletDamage);
        }

        if (collision.gameObject.CompareTag(IS_FLYING_ENEMY))
        {
            collision.gameObject.GetComponent<FlyingEnemy>().TakeDamage(playerStats.bulletDamage);
        }

        Destroy(hitFx, 1f);
        Destroy(gameObject);
    }
}
