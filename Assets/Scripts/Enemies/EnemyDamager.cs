using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float damageAmount = 1f;
    [SerializeField] private gameManager.ColorType damageColor;

    [Header("Lifetime Settings")]
    [SerializeField] private float lifeTime = 5f;

    private void Start()
    {
        if (lifeTime > 0f)
        {
            Destroy(gameObject, lifeTime);
        }
    }

    public void SetDamage(float newDamage)
    {
        damageAmount = newDamage;
    }

    public void SetLifeTime(float newLifeTime)
    {
        lifeTime = newLifeTime;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;

        EnemyBehavior enemy =
            collision.GetComponent<EnemyBehavior>();

        if (enemy == null)
        {
            enemy =
                collision.GetComponentInParent<EnemyBehavior>();
        }

        if (enemy != null)
        {
            enemy.takeDamage(
                damageAmount,
                damageColor
            );
        }
    }
}