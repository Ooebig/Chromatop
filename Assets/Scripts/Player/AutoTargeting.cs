using UnityEngine;

public class AutoTargeting : MonoBehaviour
{
    [Header("Targeting Settings")]

    [Tooltip("Maximum distance the player can detect enemies.")]
    [SerializeField] private float targetRange = 10f;

    [Tooltip("Only objects on this layer can be targeted.")]
    [SerializeField] private LayerMask enemyLayer;

    // The enemy currently selected by the targeting system.
    public Transform CurrentTarget { get; private set; }

    private void Update()
    {
        FindClosestTarget();
    }

    private void FindClosestTarget()
    {
        Collider[] enemies = Physics.OverlapSphere(
            transform.position,
            targetRange,
            enemyLayer
        );

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            float distance = Vector3.Distance(
                transform.position,
                enemy.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        CurrentTarget = closestEnemy;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, targetRange);
    }
}