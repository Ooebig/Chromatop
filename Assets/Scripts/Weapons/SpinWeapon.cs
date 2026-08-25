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
        if (newOrb == null)
            return;

        newOrb.localScale =
            Vector3.one * currentStats.area;

        damage orbDamager =
            newOrb.GetComponentInChildren<damage>(true);

        if (orbDamager == null)
        {
            Debug.LogError(
                "The spawned orb needs a damage component.",
                newOrb
            );

            Destroy(newOrb.gameObject);
            return;
        }

        PlayerStats playerStats =
            gameManager.instance.player
                .GetComponent<PlayerStats>();

        float playerDamage = playerStats != null
            ? playerStats.Damage
            : 1f;

        orbDamager.damageAmount =
            currentStats.baseDamageMult *
            playerDamage;

        orbDamager.bulletDestroyTime =
            currentStats.duration;

        // Make the orb use the player's active damage color.
        orbDamager.dmgColor =
            gameManager.instance.activeColor;

        MeshRenderer orbRenderer =
            newOrb.GetComponentInChildren<MeshRenderer>(true);

        if (orbRenderer != null)
        {
            orbRenderer.material =
                gameManager.instance.activeMaterial;
        }
        else
        {
            Debug.LogWarning(
                "The spawned orb does not have a MeshRenderer.",
                newOrb
            );
        }
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