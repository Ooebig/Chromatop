using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public int totalEnemiesInWave = 10;
        public float timeToSpawn = 1f;

        [Range(0, 100)] public float simpleRatio;
        [Range(0, 100)] public float chargerRatio;
        [Range(0, 100)] public float shooterRatio;
    }
    public GameObject enemy;
    public Transform playerPos;
    
    public List<Wave> waves;
    public float timeBetweenWaves = 5f;

    public Vector3 mapCenter = Vector3.zero;
    public float spawnRadius = 20f;
    public float minDistanceToPlayer = 5f;

    private int waveIndex = 0;
    private bool isSpawning = false;
    private List<GameObject> activeEnemies = new List<GameObject>();

    public TextMeshProUGUI waveCount;
    public TextMeshProUGUI timeToWave;

    private float currTimer;
    private void Start()
    {
        if (playerPos == null)
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (waves.Count > 0)
        {
            StartCoroutine(WaveLoop());
        }
    }
    private void Update()
    {
        UpdateUI();
    }
    IEnumerator WaveLoop()
    {
        while (waveIndex < waves.Count)
        {
            isSpawning = true;
            yield return StartCoroutine(SpawnWave(waves[waveIndex]));
            isSpawning = false;

            while (activeEnemies.Count > 0)
            {
                activeEnemies.RemoveAll(item => item == null);
                yield return new WaitForSeconds(0.5f);
            }
            currTimer = timeBetweenWaves;
            while (currTimer > 0)
            {
                yield return null;
                currTimer -= Time.deltaTime;
            }
            currTimer = 0;

            waveIndex++;
        }
    }
    EnemyBehavior.EnemyType GetTypeFromRatio(Wave wave)
    {
        float totalWeight = wave.simpleRatio + wave.shooterRatio + wave.chargerRatio;
        if (totalWeight <= 0) return EnemyBehavior.EnemyType.Simple;
        float roll = Random.Range(0f, totalWeight);

        if (roll < wave.simpleRatio)
        {
            return EnemyBehavior.EnemyType.Simple;
        }
        else if (roll < wave.shooterRatio + wave.simpleRatio)
        {
            return EnemyBehavior.EnemyType.Shooter;
        }
        else
        {
            return EnemyBehavior.EnemyType.Charger;
        }

    }
    IEnumerator SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.totalEnemiesInWave; i++)
        {
            EnemyBehavior.EnemyType randType = GetTypeFromRatio(wave);

            Vector3 validSpawn = GetValidSpawn();
            SpawnEnemy(randType, validSpawn);

            yield return new WaitForSeconds(wave.timeToSpawn);

        }
    }

    Vector3 GetValidSpawn()
    {
        Vector3 spawnPos = Vector3.zero;
        bool valid = false;

        while (!valid)
        {
            Vector2 randomCirclePoint = Random.insideUnitCircle * spawnRadius;
            spawnPos = new Vector3(randomCirclePoint.x, 0f, randomCirclePoint.y) + mapCenter;
            if (playerPos != null)
            {
                float distanceToPlayer = Vector3.Distance(spawnPos, playerPos.position);
                if (distanceToPlayer >= minDistanceToPlayer)
                {
                    valid = true;
                }
            }
            else 
            { 
                valid = true; 
            }
        }
        return spawnPos;
    }

    void SpawnEnemy(EnemyBehavior.EnemyType TypeToSpawn, Vector3 spawnPos)
    {
        GameObject newEnemy = Instantiate(enemy, spawnPos, Quaternion.identity);
        activeEnemies.Add(newEnemy);

        EnemyStats enemyStats = newEnemy.GetComponent<EnemyStats>();
        EnemyBehavior enemyBehavior = newEnemy.GetComponent<EnemyBehavior>();

        enemyStats.type = TypeToSpawn;
        enemyBehavior.player = playerPos;

        System.Array colors = System.Enum.GetValues(typeof(gameManager.ColorType));
        gameManager.ColorType randColor = (gameManager.ColorType)colors.GetValue(Random.Range(0, colors.Length - 1));
        enemyStats.Color = randColor;

        if (TypeToSpawn == EnemyBehavior.EnemyType.Simple)
        {
            enemyStats.speed = 3f;
            enemyStats.maxHp = 100f;
        }
        else if(TypeToSpawn == EnemyBehavior.EnemyType.Charger)
        {
            enemyStats.speed = 5f;
            enemyStats.maxHp = 150f;
        }
        else if(TypeToSpawn == EnemyBehavior.EnemyType.Shooter)
        {
            enemyStats.speed = 2f;
            enemyStats.maxHp = 70f;
        }
        enemyStats.LoadModel();
    }


    private void UpdateUI()
    {
        

        if(waveIndex < waves.Count)
        {
            int waveNumber = waveIndex + 1;
            int totalWaves = waves.Count;

            waveCount.text = $"Wave {waveNumber} / {totalWaves}";
        }
        else
        {
            timeToWave.text = "All waves cleared";
        }

        if(activeEnemies.Count > 0)
        {
            timeToWave.text = $"Enemies Left: {activeEnemies.Count}";
        }
        else
        {
            timeToWave.text = $"Next wave in: {Mathf.Max(0, currTimer):F1}s";
        }
    }
}
