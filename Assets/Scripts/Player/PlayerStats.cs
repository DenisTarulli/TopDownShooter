using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerHealthBar healthBar;
    [SerializeField] private PlayerExpBar expBar;
    public float bulletDamage = 15f;
    public float fireRate = 15f;
    public float maxHealth = 100f;
    public float currentHealth = 0f;
    private float maxExp = 100f;
    public float currentExperience = 0f;
    public int level = 0;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        currentExperience = 0f;
        expBar.SetMaxExp(maxExp);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    public void GainExp(float exp)
    {
        currentExperience += exp;

        if (currentExperience >= 100)
        {
            level += 1;
            currentExperience -= 100;
        }

        expBar.SetExp(currentExperience);
    }
}
