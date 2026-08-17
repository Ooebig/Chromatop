using System.Collections;
using Unity.Mathematics;
using UnityEngine;

    public class EnemyBehavior : MonoBehaviour, iDamage
    {
        public enum EnemyType { Simple, Charger, Shooter }

    
        public EnemyStats stats;
        public Transform player;
        public GameObject projectilePrefab;
        public Transform firepoint;
        public float lastShot;

        public int Team => throw new System.NotImplementedException();

        void Start()
        {
            if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
            stats = GetComponent<EnemyStats>();
            
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
                transform.position += dir * stats.speed * Time.deltaTime;
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
        lastShot = Time.time + UnityEngine.Random.Range(0f, 0.5f);
            while (true)
            {
            float disToPlayer = Vector3.Distance(transform.position, player.position);
            transform.LookAt(player.position);

            if (disToPlayer > stats.stopDistance)
            {
                Vector3 dir = (player.position - transform.position).normalized;
                transform.position += dir * stats.speed * Time.deltaTime;
            }
            if (Time.time >= lastShot + stats.firerate)
            {
                Shoot();
                lastShot = Time.time;
            }
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
                    playerDamage.takeDamage(stats.contactDamage, stats.Color);
                }
            }
        }
    private void Shoot()
    {
        Vector3 spawnPos = firepoint.position;
        quaternion spawnRot = firepoint.rotation;
        GameObject bullet = Instantiate(projectilePrefab, spawnPos, spawnRot);
    }
    }
