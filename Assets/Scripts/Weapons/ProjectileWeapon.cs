using UnityEngine;

public class ProjectileWeapon : Weapon
{
    [Header("Projectile References")]
    [SerializeField] private damage projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Targeting")]
    [SerializeField] private float weaponRange = 10f;
    [SerializeField] private LayerMask whatIsEnemy;

    private WeaponStats currentStats;
    private float shotCounter;

    private void Start()
    {
        SetStats();
    }

    private void Update()
    {
        UpdateStatsIfNeeded();
        UpdateFiring();
    }

    private void UpdateStatsIfNeeded()
    {
        if (!statsUpdated)
            return;

        statsUpdated = false;
        SetStats();
    }

    private void UpdateFiring()
    {
        if (currentStats == null || projectilePrefab == null)
            return;

        shotCounter -= Time.deltaTime;

        if (shotCounter > 0f)
            return;

        FireAtEnemies();

        float finalCooldown =
            playerStats != null
                ? playerStats.Calculate(
                    StatType.Cooldown,
                    currentStats.cooldown
                )

                : currentStats.cooldown;

        shotCounter = Mathf.Max(
            0.01f,
            finalCooldown
        );
    }

    private void FireAtEnemies()
    {
        Vector3 origin = firePoint != null
            ? firePoint.position
            : transform.position;

        float detectionRange =
            weaponRange * currentStats.area;

        Collider[] enemies = Physics.OverlapSphere(
            origin,
            detectionRange,
            whatIsEnemy,
            QueryTriggerInteraction.Collide
        );

        if (enemies.Length == 0)
            return;

        System.Collections.Generic.List<Collider> validEnemies =
            new System.Collections.Generic.List<Collider>();

        foreach (Collider enemy in enemies)
        {
            if (enemy.GetComponentInParent<EnemyStats>() != null)
                validEnemies.Add(enemy);
        }

        if (validEnemies.Count == 0)
            return;

        for (int i = 0; i < currentStats.amount; i++)
        {
            Collider target =
                validEnemies[Random.Range(0, validEnemies.Count)];

            FireProjectileAt(target, origin);
        }
    }

    private void FireProjectileAt(
        Collider target,
        Vector3 spawnPosition
    )
    {
        Vector3 direction =
            target.bounds.center - spawnPosition;

        // Keep aiming flat across the X/Z ground plane.
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
            return;

        Quaternion spawnRotation =
            Quaternion.LookRotation(
                direction.normalized,
                Vector3.up
            );

        damage newProjectile = Instantiate(
            projectilePrefab,
            spawnPosition,
            spawnRotation
        );

        ConfigureProjectile(newProjectile);

        newProjectile.gameObject.SetActive(true);
    }

    private void ConfigureProjectile(
    damage newProjectile
)
    {
        if (newProjectile == null ||
            currentStats == null)
        {
            return;
        }

        gameManager manager =
    gameManager.instance;

        if (manager == null)
        {
            manager =
                FindFirstObjectByType<gameManager>();
        }

        gameManager.ColorType weaponColor =
            manager != null
                ? manager.activeColor
                : gameManager.ColorType.GREY;

        Material weaponMaterial =
            manager != null
                ? manager.activeMaterial
                : null;

        float finalDamage =
            currentStats.baseDamageMult *
            (playerStats != null
                ? playerStats.Damage
                : 1f);

        newProjectile.transform.localScale =
            Vector3.one * currentStats.area;

        newProjectile.Configure(
        finalDamage,
            currentStats.speed,
            currentStats.duration,
            weaponColor,
            1,
            weaponMaterial
);
    }

    private void SetStats()
    {
        currentStats = CurrentStats;

        if (currentStats == null)
        {
            Debug.LogWarning(
                "ProjectileWeapon does not have any weapon stats.",
                this
            );

            return;
        }

        shotCounter = 0f;
    }

    private void OnDrawGizmosSelected()
    {
        float range = weaponRange;

        if (currentStats != null)
        {
            range *= currentStats.area;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            range
        );
    }
}