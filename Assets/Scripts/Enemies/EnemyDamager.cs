using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    [SerializeField] private float damageAmount = 1f;
    [SerializeField] private gameManager.ColorType damageColor;

    public void SetDamage(float newDamage)
    {
        damageAmount = newDamage;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;

        EnemyBehavior enemy =
            collision.GetComponent<EnemyBehavior>();

        if (enemy == null)
        {
            enemy = collision.GetComponentInParent<EnemyBehavior>();
        }

        if (enemy != null)
        {
            enemy.takeDamage(damageAmount, damageColor);
        }
    }
}