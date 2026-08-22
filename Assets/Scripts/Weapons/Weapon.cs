using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Progression")]
    [Tooltip("One entry for every weapon level.")]
    public List<WeaponStats> stats = new List<WeaponStats>();

    [Tooltip("Current stats-list index. Level 0 uses Element 0.")]
    [Min(0)]
    public int weaponLevel;

    [HideInInspector]
    public bool statsUpdated;

    protected PlayerStats playerStats;

    protected virtual void Awake()
    {
        playerStats = GetComponentInParent<PlayerStats>();
    }

    public WeaponStats CurrentStats
    {
        get
        {
            if (stats == null || stats.Count == 0)
                return null;

            weaponLevel = Mathf.Clamp(
                weaponLevel,
                0,
                stats.Count - 1
            );

            return stats[weaponLevel];
        }
    }

    public void UpgradeWeapon()
    {
        if (stats == null || stats.Count == 0)
            return;

        if (weaponLevel >= stats.Count - 1)
            return;

        weaponLevel++;
        statsUpdated = true;
    }

    public void SetWeaponLevel(int newLevel)
    {
        if (stats == null || stats.Count == 0)
            return;

        weaponLevel = Mathf.Clamp(
            newLevel,
            0,
            stats.Count - 1
        );

        statsUpdated = true;
    }
}

[System.Serializable]
public class WeaponStats
{
    [Header("Identification")]

    [Tooltip("Label shown in the Inspector, such as Level 1 or Rapid Orbs.")]
    public string levelName = "Level 1";

    [Header("Damage Multiplier")]

    [Tooltip("How much damage the weapon deals, multiplied off the player's base damage.")]
    [FormerlySerializedAs("damage")]
    [Min(0f)]
    public float baseDamageMult = 1f; //Changed from damage to baseDamageMult so it can scale with player damage

    [Tooltip("Force applied when the weapon hits an enemy.")]
    [Min(0f)]
    public float knockback;

    [Header("Size and Movement")]

    [Tooltip("Multiplier applied to the weapon or projectile size.")]
    [FormerlySerializedAs("range")]
    [Min(0.01f)]
    public float area = 1f;

    [Tooltip("Projectile movement or weapon rotation speed.")]
    [Min(0f)]
    public float speed = 100f;

    [Header("Attack Timing")]

    [Tooltip("Seconds before the weapon can begin another attack.")]
    [FormerlySerializedAs("timeBetweenAttacks")]
    [Min(0.01f)]
    public float cooldown = 1f;

    [Tooltip("Delay between individual projectiles in the same attack.")]
    [Min(0f)]
    public float interval = 0.1f;

    [Tooltip("How long the weapon or projectile remains active.")]
    [Min(0.01f)]
    public float duration = 5f;

    [Header("Projectile Settings")]

    [Tooltip("Number of projectiles or weapon objects created per attack.")]
    [Min(1)]
    public int amount = 1;
}