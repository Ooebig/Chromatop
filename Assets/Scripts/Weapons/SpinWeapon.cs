using UnityEngine;

public class SpinWeapon : Weapon
{
    [SerializeField] private Transform player;
    [SerializeField] private EnemyDamager damager;

    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private float orbitDistance = 2f;

    private float currentAngle;

    private void Start()
    {
        SetStats();
    }

    private void Update()
    {
        if (player == null)
            return;

        currentAngle += rotateSpeed * Time.deltaTime;
        float angle = currentAngle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(
            Mathf.Cos(angle) * orbitDistance,
            0f,
            Mathf.Sin(angle) * orbitDistance
        );

        transform.position = player.position + offset;
    }

    public void SetStats()
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

        transform.localScale =
            Vector3.one * currentStats.range;
    }
}