using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyBehavior.EnemyType type;
    // Update is called once per frame
    public float maxHp = 100f;
    public float currentHp;
    public float speed;
    public float damage;
    void Awake()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(float damage)

    {
        currentHp -= damage;

        if (currentHp < 0)
        {
            Destroy(gameObject);
        }
    }
}
