using UnityEngine;

public class SpinWeapon : Weapon
{
    [Header("Orb References")]
    [SerializeField] private Transform holder;
    [SerializeField] private Transform orbToSpawn;
    [SerializeField] private Transform orbSpawnPoint;

    [Header("Runtime Settings")]
    [SerializeField] private float rotateSpeed = 100f;

    private WeaponStats currentStats;
    private float spawnCooldown;
    private float spawnCounter;

    private void Start()
    {
        SetStats();
    }

    private void Update()
    {
        UpdateStatsIfNeeded();
        RotateHolder();
        UpdateOrbSpawning();
    }

    private void UpdateStatsIfNeeded()
    {
        if (!statsUpdated)
            return;

        statsUpdated = false;
        SetStats();
    }

    private void RotateHolder()
    {
        if (holder == null)
            return;

        holder.Rotate(
            Vector3.up,
            rotateSpeed * Time.deltaTime,
            Space.Self
        );
    }

    private void UpdateOrbSpawning()
    {
        if (!CanSpawnOrb())
            return;

        spawnCounter -= Time.deltaTime;

        if (spawnCounter > 0f)
            return;

        SpawnOrb();

        spawnCounter = spawnCooldown;
    }

    private bool CanSpawnOrb()
    {
        return holder != null &&
               orbToSpawn != null &&
               orbSpawnPoint != null &&
               currentStats != null;
    }

    private void SpawnOrb()
    {
        Transform newOrb = Instantiate(
            orbToSpawn,
            orbSpawnPoint.position,
            orbSpawnPoint.rotation,
            holder
        );
        ConfigureOrb(newOrb);

        newOrb.gameObject.SetActive(true);
    }

    private void ConfigureOrb(Transform newOrb)
    {
        if (newOrb == null || currentStats == null)
            return;

        damage orbDamage =
            newOrb.GetComponentInChildren<damage>(true);

        if (orbDamage == null)
        {
            Debug.LogError(
                "The spawned orb needs a damage component.",
                newOrb
            );

            Destroy(newOrb.gameObject);
            return;
        }

        gameManager manager =
            gameManager.instance;

        if (manager == null)
        {
            manager =
                FindAnyObjectByType<gameManager>();
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

        newOrb.localScale =
            Vector3.one * currentStats.area;

        orbDamage.Configure(
            finalDamage,
            0f,
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
                "SpinWeapon does not have any weapon stats.",
                this
            );

            return;
        }

        rotateSpeed = currentStats.speed;

        spawnCooldown = Mathf.Max(
            0.01f,
            currentStats.cooldown
        );

        // Spawn immediately after starting or upgrading.
        spawnCounter = 0f;
    }
}