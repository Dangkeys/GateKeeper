using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveHandler : MonoBehaviour
{
    [SerializeField] private WaveSettingsSO settings;
    [SerializeField] private Transform playerTransform;

    private EnemyStatModifiers enemyStatModifiers = new EnemyStatModifiers();
    private List<(EnemySpawnConfig config, float weight)> currentWavePool = new List<(EnemySpawnConfig, float)>();

    private int currentBudget;
    private int waveNumber = 1;

    private void Start()
    {
        if (settings == null)
        {
            Debug.LogError("WaveHandler: No WaveSettingsSO assigned!");
            return;
        }
        
        if (playerTransform == null) playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        StartNextWave();
    }

    public void StartNextWave()
    {

        currentBudget = Mathf.Min(settings.baseWaveBudget + (waveNumber * settings.budgetIncreasePerWave), settings.maxWaveBudget);


        enemyStatModifiers.healthMultiplier = Mathf.Min(1f + (waveNumber * settings.waveConfig.healthMultiplier), settings.waveConfig.maxHealthMultiplier);
        enemyStatModifiers.damageMultiplier = Mathf.Min(1f + (waveNumber * settings.waveConfig.damageMultiplier), settings.waveConfig.maxDamageMultiplier);
        enemyStatModifiers.moveSpeedMultiplier = Mathf.Min(1f + (waveNumber * settings.waveConfig.moveSpeedMultiplier), settings.waveConfig.maxMoveSpeedMultiplier);


        currentWavePool.Clear();
        foreach (var enemy in settings.enemyPool)
        {
            float calculatedWeight = enemy.baseWeight + (waveNumber * enemy.weightIncreasePerWave);
            currentWavePool.Add((enemy, calculatedWeight));
        }

        StopAllCoroutines();
        StartCoroutine(FairSpawnRoutine());
        waveNumber++;
    }

    private IEnumerator FairSpawnRoutine()
    {
        while (currentBudget > 0)
        {
            EnemySpawnConfig selectedEnemy = GetWeightedRandomEnemy();
            if (selectedEnemy == null) break;

            Transform spawnPoint = GetFairSpawnPoint();
            if (spawnPoint != null)
            {
                SpawnEnemy(selectedEnemy.stat, spawnPoint);
                currentBudget -= selectedEnemy.cost;
            }

            yield return new WaitForSeconds(settings.spawnDelay);
        }
    }

    private EnemySpawnConfig GetWeightedRandomEnemy()
    {
        float totalWeight = 0f;
        for (int i = 0; i < currentWavePool.Count; i++)
        {
            if (currentWavePool[i].config.cost <= currentBudget)
                totalWeight += currentWavePool[i].weight;
        }

        if (totalWeight <= 0) return null;

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        for (int i = 0; i < currentWavePool.Count; i++)
        {
            if (currentWavePool[i].config.cost <= currentBudget)
            {
                cumulative += currentWavePool[i].weight;
                if (roll <= cumulative) return currentWavePool[i].config;
            }
        }
        return null;
    }

    private Transform GetFairSpawnPoint()
    {
        var points = SpawnPoint.SpawnPointList;
        if (points == null || points.Count == 0) return null;

        List<SpawnPoint> validPoints = new List<SpawnPoint>();
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 dir = (points[i].transform.position - playerTransform.position).normalized;
            float dot = Vector3.Dot(playerTransform.forward, dir);
            float dist = Vector3.Distance(points[i].transform.position, playerTransform.position);

            if (dot > -0.5f && dist > 8f) validPoints.Add(points[i]);
        }

        var source = validPoints.Count > 0 ? validPoints : points;
        return source[Random.Range(0, source.Count)].transform;
    }

    private void SpawnEnemy(EnemyStatSO stat, Transform spot)
    {
        Vector2 offset = Random.insideUnitCircle * settings.randomSpawnOffset;
        Vector3 pos = spot.position + new Vector3(offset.x, 0, offset.y);
        
        Enemy enemy = Instantiate(settings.enemyPrefab, pos, spot.rotation);
        enemy.Initialize(stat, enemyStatModifiers);
    }
}