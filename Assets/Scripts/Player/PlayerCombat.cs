using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private float fireRate = 1f;

    [Header("References")]
    [SerializeField] private AutoTargeting autoTargeting;

    private float fireTimer;

    private void Update()
    {
        fireTimer += Time.deltaTime;

        if (autoTargeting.CurrentTarget == null)
            return;

        if (fireTimer >= fireRate)
        {
            Fire();
            fireTimer = 0f;
        }
    }

    private void Fire()
    {
        Transform target = autoTargeting.CurrentTarget;

        Vector3 attackDirection =
            (target.position - transform.position).normalized;

        Debug.DrawRay(
            transform.position,
            attackDirection * 10f,
            Color.red,
            0.5f
        );

        Debug.Log("Attacking: " + target.name);
    }
}