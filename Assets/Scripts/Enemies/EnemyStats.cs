using UnityEngine;
using UnityEngine.Rendering;


public class EnemyStats : MonoBehaviour
{
    public EnemyBehavior.EnemyType type;
    public gameManager.ColorType Color;
    // Update is called once per frame
    
    public float maxHp = 100f;
    public float currentHp;
    public float speed;
    public float damage;
    void Awake()
    {
        currentHp = maxHp;
    }

    
}
