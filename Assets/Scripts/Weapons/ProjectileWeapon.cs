using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UIElements;

public class ProjectileWeapon : Weapon
{
    [SerializeField] private EnemyDamager damager;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Targeting")]
    [SerializeField] private float weaponRange = 10f;
    [SerializeField] private LayerMask whatIsEnemy;

    private float shotCounter;

    private void Start()
    {
        SetStats();
    }

    private void Update()
    {
        if (statsUpdated)
        {
            statsUpdated = false;
            SetStats();
        }

        shotCounter -= Time.deltaTime;

        if (shotCounter <= 0f)
        {
            FireAtEnemies();

            shotCounter =
                stats[weaponLevel].timeBetweenAttacks;
        }
    }

    private void FireAtEnemies()
    {
        Collider[] enemies = Physics.OverlapSphere(
            transform.position,
            weaponRange * stats[weaponLevel].range,
            whatIsEnemy
        );

        if (enemies.Length == 0)
            return;

        for (int i = 0; i < stats[weaponLevel].amount; i++)
        {
            Collider target =
                enemies[Random.Range(0, enemies.Length)];

            Vector3 spawnPosition = firePoint != null
                ? firePoint.position
                : transform.position;

            Vector3 direction =
                target.transform.position - spawnPosition;

            // Keep the projectile aimed across the X/Z ground plane.
            direction.y = 0f;

            if (direction.sqrMagnitude == 0f)
                continue;

            Quaternion projectileRotation =
                Quaternion.LookRotation(direction.normalized, Vector3.up);

            Projectile newProjectile = Instantiate(
                projectilePrefab,
                spawnPosition,
                projectileRotation
            );

            newProjectile.gameObject.SetActive(true);
        }
    }

    private void SetStats()
    {
        if (stats == null || stats.Count == 0)
            return;

        weaponLevel = Mathf.Clamp(
            weaponLevel,
            0,
            stats.Count - 1
        );

        WeaponStats currentStats = stats[weaponLevel];

        if (damager != null)
        {
            damager.SetDamage(currentStats.damage);
        }

        if (projectilePrefab != null)
        {
            projectilePrefab.moveSpeed = currentStats.speed;

            projectilePrefab.transform.localScale =
                Vector3.one * currentStats.range;

            projectilePrefab.SetLifeTime(currentStats.duration);
        }

        shotCounter = 0f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, weaponRange);
    }
}