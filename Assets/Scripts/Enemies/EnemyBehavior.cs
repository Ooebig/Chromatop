using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public enum EnemyType { Simple, Charger, Shooter }
    

    public EnemyStats stats;
    public Transform player;
    public float speed = 3.0f;
    void Start()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
        stats = GetComponent<EnemyStats>();
        stats.speed = speed;
        switch (stats.type)
        {
            case EnemyType.Simple:
                StartCoroutine(SimpleBehavior());
                break;
            case EnemyType.Charger:
                StartCoroutine(ChargerBehavior());
                break;
            case EnemyType.Shooter:
                StartCoroutine(ShooterBehavior());
                break;
        }
    }

    IEnumerator SimpleBehavior()
    {
        while (true)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;
            transform.LookAt(player.position);
            yield return null;
        }
    }

    IEnumerator ChargerBehavior()
    {
        while (true)
        {

            yield return null;

        }
    }

    IEnumerator ShooterBehavior()
    {
        while (true)
        {

            yield return null;

        }
    }
}
