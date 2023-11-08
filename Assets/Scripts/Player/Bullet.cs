using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject hitEffect;
    //private const string IS_ENEMY = "Enemy";    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hitFx = Instantiate(hitEffect, transform.position, Quaternion.identity);

        Destroy(hitFx, 1f);
        Destroy(gameObject);
    }
}
