using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer gunRenderer;
    private PlayerStats stats;
    private Transform aimTransform;

    [SerializeField] private float bulletForce = 8f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    private float nextTimeToFire = 0f;

    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        stats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        HandleAiming();

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / stats.fireRate;
            HandleShooting();
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
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        bulletRb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);

        Destroy(bullet, 1.5f);
    }
}
