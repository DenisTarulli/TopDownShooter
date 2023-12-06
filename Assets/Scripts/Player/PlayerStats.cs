using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerHealthBar healthBar;
    [SerializeField] private PlayerExpBar expBar;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject levelUpEffect;
    [SerializeField] private Transform lvlEffectTransform;
    [SerializeField] private GameObject damageEffect;
    [SerializeField] private Transform canvas;
    public bool isImmune = false;
    public float bulletDamage = 15f;
    public float fireRate = 15f;
    public float maxHealth = 100f;
    public float currentHealth = 0f;
    private float maxExp = 100f;
    public float currentExperience = 0f;
    public float totalExperience = 0f;
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
        FindObjectOfType<AudioManager>().Play("LevelUp");

        bulletDamage += 0.3f;
        fireRate += 1.2f;
        playerActions.moveSpeed += 0.2f;

        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);

        maxExp += Mathf.Pow((float)(level), 3);
        expBar.SetMaxExp(maxExp);

        GameObject lvlup = Instantiate(levelUpEffect, lvlEffectTransform);

        Destroy(lvlup, 1.2f);
    }

    public void TakeDamage(float damage)
    {
        GameObject dmgEffect = Instantiate(damageEffect, canvas);
        Destroy(dmgEffect, 1f);

        FindObjectOfType<AudioManager>().Play("Hurt");

        StartCoroutine(ImmunityTime());

        currentHealth -= damage;
        hitsTaken += 1;
        healthBar.SetHealth(currentHealth);
    }

    public void GainExp(float exp)
    {
        currentExperience += exp;
        totalExperience += exp;

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
