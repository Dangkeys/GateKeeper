using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

public class WaveHandler : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private float randomSpawnOffset = 30f;

    [Header("Enemy Pool & Weights")]
    public List<EnemySpawnConfig> enemyPool;

    [Header("Wave Pacing & Budget")]
    [SerializeField] private int baseWaveBudget = 20;
    [SerializeField] private int budgetIncreasePerWave = 15;
    [SerializeField] private int maxWaveBudget = 150;
    [SerializeField] private float spawnDelay = 0.5f;

    [Header("Enemy Stat Scaling")]
    [SerializeField] private EnemyWaveConfig waveConfig;

    private int currentBudget;
    private int waveNumber = 1;
    private EnemyStatModifiers enemyStatModifiers = new EnemyStatModifiers();
    private List<(EnemySpawnConfig config, float weight)> currentWavePool = new List<(EnemySpawnConfig, float)>();

    private void Start()
    {
        StartNextWave();
    }

    public void StartNextWave()
    {
        currentBudget = Mathf.Min(baseWaveBudget + (waveNumber * budgetIncreasePerWave), maxWaveBudget);

        enemyStatModifiers.healthMultiplier = Mathf.Min(1f + (waveNumber * waveConfig.healthMultiplier), waveConfig.maxHealth);
        enemyStatModifiers.damageMultiplier = Mathf.Min(1f + (waveNumber * waveConfig.damageMultiplier), waveConfig.maxDamage);
        enemyStatModifiers.moveSpeedMultiplier = Mathf.Min(1f + (waveNumber * waveConfig.moveSpeedMultiplier), waveConfig.maxMoveSpeed);

        currentWavePool.Clear();
        foreach (var enemy in enemyPool)
        {
            float calculatedWeight = enemy.baseWeight + (waveNumber * enemy.weightIncreasePerWave);
            currentWavePool.Add((enemy, calculatedWeight));
        }

        StartCoroutine(FairSpawnRoutine());
        waveNumber++;
    }

    private IEnumerator FairSpawnRoutine()
    {
        while (currentBudget > 0)
        {
            var affordableEnemies = currentWavePool.Where(e => e.config.cost <= currentBudget).ToList();
            if (affordableEnemies.Count == 0) break;

            EnemySpawnConfig selectedEnemy = GetWeightedRandomEnemy(affordableEnemies);
            Transform spawnPoint = GetFairSpawnPoint();

            if (spawnPoint != null)
            {
                SpawnEnemy(selectedEnemy.stat, spawnPoint);
                currentBudget -= selectedEnemy.cost;
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private EnemySpawnConfig GetWeightedRandomEnemy(List<(EnemySpawnConfig config, float weight)> affordableEnemies)
    {
        float totalWeight = affordableEnemies.Sum(e => e.weight);
        float randomRoll = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var enemy in affordableEnemies)
        {
            cumulativeWeight += enemy.weight;
            if (randomRoll <= cumulativeWeight)
            {
                return enemy.config;
            }
        }

        return affordableEnemies[0].config;
    }

    private Transform GetFairSpawnPoint()
    {
        var points = SpawnPoint.SpawnPointList;
        if (points == null || points.Count == 0) return null;

        var fairPoints = points.Where(p =>
        {
            Vector3 directionToPoint = (p.transform.position - playerTransform.position).normalized;
            float dot = Vector3.Dot(playerTransform.forward, directionToPoint);
            float dist = Vector3.Distance(p.transform.position, playerTransform.position);

            return dot > -0.5f && dist > 8f;
        }).ToList();

        var validPoints = fairPoints.Count > 0 ? fairPoints : points;
        return validPoints[Random.Range(0, validPoints.Count)].transform;
    }

    private void SpawnEnemy(EnemyStatSO stat, Transform spot)
    {
        Vector2 randomOffset = Random.insideUnitCircle * randomSpawnOffset;
        Vector3 spawnPosition = spot.position + new Vector3(randomOffset.x, 0, randomOffset.y);
        
        Enemy enemy = Instantiate(enemyPrefab, spawnPosition, spot.rotation);
        enemy.Initialize(stat, enemyStatModifiers);
    }
}

[System.Serializable]
public class EnemySpawnConfig
{
    [FormerlySerializedAs("data")] 
    public EnemyStatSO stat;
    public int cost;
    public float baseWeight = 100f;
    public float weightIncreasePerWave = 0f;
}

[System.Serializable]
public class EnemyWaveConfig
{
    public float healthMultiplier = 0.2f;
    public float damageMultiplier = 0.15f;
    public float moveSpeedMultiplier = 0.05f;
    public float maxHealth = 3.0f;
    public float maxDamage = 2.5f;
    public float maxMoveSpeed = 1.5f;
}

[System.Serializable]
public struct EnemyStatModifiers
{
    public float healthMultiplier;
    public float damageMultiplier;
    public float moveSpeedMultiplier;

    public EnemyStatModifiers(float health, float damage, float speed)
    {
        healthMultiplier = health;
        damageMultiplier = damage;
        moveSpeedMultiplier = speed;
    }
}