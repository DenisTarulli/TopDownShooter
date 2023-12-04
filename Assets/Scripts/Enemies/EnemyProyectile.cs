using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProyectile : MonoBehaviour
{
    [SerializeField] private float damage = 15f;
    private const string IS_PLAYER = "Player";
    private const string IS_WALL = "Wall";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(IS_PLAYER))
        {
            collision.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag(IS_WALL))
            Destroy(gameObject);
    }    
}
