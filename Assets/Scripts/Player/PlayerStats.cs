using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public float bulletDamage = 15f;
    public float fireRate = 15f;
    public float maxHealth = 100f;

    [Header("References")]
    [SerializeField] private PlayerHealthBar healthBar;
    [SerializeField] private PlayerExpBar expBar;
    [SerializeField] private GameObject levelUpEffect;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Transform lvlEffectTransform;
    [SerializeField] private GameObject levelEffect;
    [SerializeField] private GameObject damageEffect;
    [SerializeField] private Transform canvas;

    // Accesible variables
    [HideInInspector] public bool isImmune = false;
    [HideInInspector] public float currentHealth = 0f;
    [HideInInspector] public float currentExperience = 0f;
    [HideInInspector] public float totalExperience = 0f;
    [HideInInspector] public int level = 0;
    [HideInInspector] public int totalKills = 0;
    [HideInInspector] public int hitsTaken = 0;

    // Private references & variables
    private float maxExp = 100f;
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

        GameObject lvlupfx = Instantiate(levelEffect, canvas);

        Destroy(lvlupfx, 1.2f);

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
