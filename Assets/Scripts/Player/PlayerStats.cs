using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerHealthBar healthBar;
    [SerializeField] private PlayerExpBar expBar;
    [SerializeField] private TextMeshProUGUI levelText;
    public bool isImmune = false;
    public float bulletDamage = 15f;
    public float fireRate = 15f;
    public float maxHealth = 100f;
    public float currentHealth = 0f;
    private float maxExp = 100f;
    public float currentExperience = 0f;
    public int level = 0;
    public int totalKills = 0;
    public int hitsTaken = 0;


    private PlayerActions playerActions;

    private void Start()
    {
        playerActions = GetComponent<PlayerActions>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        currentExperience = 0f;
        expBar.SetMaxExp(maxExp);
    }

    private void LevelUp()
    {
        bulletDamage += 2;
        fireRate += 1.5f;
        playerActions.moveSpeed += 0.5f;

        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);

        maxExp += 10;
        expBar.SetMaxExp(maxExp);
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(ImmunityTime());

        currentHealth -= damage;
        hitsTaken += 1;
        healthBar.SetHealth(currentHealth);
    }

    public void GainExp(float exp)
    {
        currentExperience += exp;

        if (currentExperience >= maxExp)
        {
            level += 1;
            levelText.text = level.ToString();
            currentExperience -= maxExp;

            LevelUp();
        }

        expBar.SetExp(currentExperience);
    }

    public IEnumerator ImmunityTime()
    {
        isImmune = true;

        yield return new WaitForSeconds(0.35f);

        isImmune = false;
    }
}
