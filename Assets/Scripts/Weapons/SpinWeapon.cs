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
        newOrb.localScale =
            Vector3.one * currentStats.area;

        EnemyDamager orbDamager =
            newOrb.GetComponentInChildren<EnemyDamager>(true);

        if (orbDamager == null)
        {
            Debug.LogError(
                "The spawned orb needs an EnemyDamager component.",
                newOrb
            );

            return;
        }

        orbDamager.SetDamage(currentStats.baseDamage);
        orbDamager.SetLifeTime(currentStats.duration);
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