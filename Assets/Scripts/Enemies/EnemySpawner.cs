using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public enum Difficulty
        {easy, normal, hard, boss}
        public Difficulty difficulty;
        public int totalEnemiesInWave = 10;
        public float timeToSpawn = 1f;

        [Range(0, 100)] public float simpleRatio;
        [Range(0, 100)] public float chargerRatio;
        [Range(0, 100)] public float shooterRatio;
        public float Multiplier = 1f;
    }
    public GameObject enemy;
    public Transform playerPos;
    public float timeBetweenWaves = 5f;
    public Vector3 mapCenter = Vector3.zero;
    public float spawnRadius = 20f;
    public float minDistanceToPlayer = 5f;

    
    private bool isSpawning = false;
    private List<GameObject> activeEnemies = new List<GameObject>();

    
    public TextMeshProUGUI waveTimerText;
    public TMP_Text difficultyText;


    private float waveTimer;
    private int currentWaveIndex;
    private void Start()
    {
        if (playerPos == null)
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        }
       
    }
    private void Update()
    {
        UpdateUI();

        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    StartWave(Wave.Difficulty.normal, 60f);
        //}
    }
    public void StartWave(Wave.Difficulty difficulty, float waveDuration)
    {

        currentWaveIndex++;

        Wave dynamic = new Wave();
        dynamic.difficulty = difficulty;

        switch (difficulty)
        {
            case Wave.Difficulty.easy:
                dynamic.timeToSpawn = 1.5f;
                dynamic.simpleRatio = 80f;
                dynamic.chargerRatio = 10f;
                dynamic.shooterRatio = 10f;
                dynamic.Multiplier = 0.75f;
                difficultyText.text = "Easy";
                break;
            case Wave.Difficulty.normal:
                dynamic.timeToSpawn = 1.0f;
                dynamic.simpleRatio = 50f;
                dynamic.chargerRatio = 25f;
                dynamic.shooterRatio = 25f;
                dynamic.Multiplier = 1f;
                difficultyText.text = "Normal";
                break;
            case Wave.Difficulty.hard:
                dynamic.timeToSpawn = 0.75f;
                dynamic.simpleRatio = 40f;
                dynamic.chargerRatio = 30f;
                dynamic.shooterRatio = 30f;
                dynamic.Multiplier = 1.5f;
                difficultyText.text = "Hard";
                break;
            case Wave.Difficulty.boss:
                dynamic.timeToSpawn = 0.5f;
                dynamic.simpleRatio = 20;
                dynamic.chargerRatio = 40f;
                dynamic.shooterRatio = 40f;
                dynamic.Multiplier = 2.0f;
                difficultyText.text = "Boss";
                break;

        }

        StopAllCoroutines();
        StartCoroutine(WaveCountdown(waveDuration));
        StartCoroutine(WaveLoop(dynamic));
    }
    IEnumerator WaveLoop(Wave wave)
    {
        isSpawning = true;
        while(waveTimer > 0)
        {
            EnemyBehavior.EnemyType randType = GetTypeFromRatio(wave);
            Vector3 validSpawn = GetValidSpawn();
            SpawnEnemy(randType, validSpawn, wave.Multiplier);
            yield return new WaitForSeconds(wave.timeToSpawn);
        }
        isSpawning = false;
        gameManager.instance.RoomEnd();
        while (activeEnemies.Count > 0)
        {
            activeEnemies.RemoveAll(item => item == null);
            yield return new WaitForSeconds(0.5f);
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
        while (isSpawning)
        {
            EnemyBehavior.EnemyType randType = GetTypeFromRatio(wave);

            Vector3 validSpawn = GetValidSpawn();
            SpawnEnemy(randType, validSpawn, wave.Multiplier);

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
            spawnPos = new Vector3(randomCirclePoint.x, playerPos.position.y, randomCirclePoint.y) + mapCenter;
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

    void SpawnEnemy(EnemyBehavior.EnemyType TypeToSpawn, Vector3 spawnPos, float Multiplier)
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
            enemyStats.maxHp = 100f;
            enemyStats.speed = 3f;
            enemyStats.damage = 10f;
            enemyStats.firerate = 0f;
            enemyStats.stopDistance = 0f;
            enemyStats.contactDamage = 10f;
            enemyStats.maxCharge = 0f;
            enemyStats.chargeDelay = 0f;
            enemyStats.currentHp = enemyStats.maxHp;
        }
        else if(TypeToSpawn == EnemyBehavior.EnemyType.Charger)
        {
            enemyStats.maxHp = 150f;
            enemyStats.speed = 5f;
            enemyStats.damage = 15f;
            enemyStats.firerate = 0f;
            enemyStats.stopDistance = 0f;
            enemyStats.contactDamage = 20f;
            enemyStats.chargeDelay = 3f;
            enemyStats.maxCharge = 1.5f;
            enemyStats.currentHp = enemyStats.maxHp;
        }
        else if(TypeToSpawn == EnemyBehavior.EnemyType.Shooter)
        {

            enemyStats.firerate = 1.5f;
            enemyStats.stopDistance = 7f;
            enemyStats.speed = 2f;
            enemyStats.maxHp = 70f;
            enemyStats.currentHp = enemyStats.maxHp;
        }

        enemyStats.firerate *= Multiplier;
        enemyStats.damage *= Multiplier;
        enemyStats.speed *= Multiplier;
        enemyStats.maxHp *= Multiplier;
        enemyStats.currentHp = enemyStats.maxHp;



        enemyStats.LoadModel();
    }
    public void ClearWave()
    {
        StopAllCoroutines();
        isSpawning = false;

        foreach (GameObject obj in activeEnemies)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        activeEnemies.Clear();

        foreach (GameObject drop in GameObject.FindGameObjectsWithTag("Drop"))
        {
            Destroy(drop);
        }

        
    }

    IEnumerator WaveCountdown(float duration)
    {
        waveTimer = duration;
        while (waveTimer > 0)
        {
            waveTimer -= Time.deltaTime;
            yield return null;
        }

        waveTimer = 0;
    }
    private void UpdateUI()
    {
        if (waveTimerText != null)
        {
            waveTimerText.text = "Time: " + Mathf.Ceil(waveTimer).ToString();
        }

      
    }
}
