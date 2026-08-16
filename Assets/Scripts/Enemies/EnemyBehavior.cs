using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour, iDamage
{
    public enum EnemyType { Simple, Charger, Shooter }

    
    public EnemyStats stats;
    public Transform player;
    public float speed = 3.0f;
    public float contactDamage = 5f;

    public int Team => throw new System.NotImplementedException();

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

    public void takeDamage(float amount, gameManager.ColorType dmgColor)
    {
        float damage = gameManager.damageCalc(amount, stats.Color, dmgColor);
        stats.currentHp -= damage;

        if (stats.currentHp < 0) 
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enemy hit something: " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            iDamage playerDamage = other.GetComponent<iDamage>();

            if (playerDamage != null)
            {
                playerDamage.takeDamage(contactDamage, stats.Color);
            }
        }
    }
}
