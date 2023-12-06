using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer gunRenderer;
    [SerializeField] private PauseMenu pauseMenu;
    private PlayerStats playerStats;
    private Transform aimTransform;
    private GameManager gameManager;

    [SerializeField] private float bulletForce = 8f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    private float nextTimeToFire = 0f;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        aimTransform = transform.Find("Aim");
        playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (!pauseMenu.gameIsPaused && gameManager.gameStarted)
        {
            HandleAiming();

            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / playerStats.fireRate;
                HandleShooting();
            }
        }        
    }

    private void HandleAiming()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
        if (Mathf.Abs(angle) > 90)
            gunRenderer.flipY = true;
        else
            gunRenderer.flipY = false;        
    }

    private void HandleShooting()
    {
        FindObjectOfType<AudioManager>().Play("Shoot");

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        bulletRb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);        

        Destroy(bullet, 1.5f);
    }
}
