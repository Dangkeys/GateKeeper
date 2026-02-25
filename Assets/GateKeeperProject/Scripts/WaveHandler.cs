using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaveHandler : MonoBehaviour
{
    [Header("Enemy Tier Config")]
    public List<EnemySpawnConfig> enemyPool;

    [Header("Player Reference")]
    public Transform playerTransform; // Assign Main Camera
    [SerializeField] private Enemy enemyPrefab;

    [SerializeField] private float randomSpawnOffset = 3f;

    private int currentBudget;
    private int waveNumber = 1;

    void Start()
    {
        StartNextWave();
    }

    public void StartNextWave()
    {
        // Increase difficulty: 20 base + 15 per wave
        currentBudget = 20 + (waveNumber * 15);
        StartCoroutine(FairSpawnRoutine());
        waveNumber++;
    }

    private IEnumerator FairSpawnRoutine()
    {
        while (currentBudget > 0)
        {
            // 1. Pick a random enemy we can afford
            var affordable = enemyPool.Where(e => e.cost <= currentBudget).ToList();
            if (affordable.Count == 0) break;

            EnemySpawnConfig selection = affordable[Random.Range(0, affordable.Count)];

            // 2. Find a "Fair" Spawn Point from your SpawnPointList
            Transform bestPoint = GetFairSpawnPoint();

            if (bestPoint != null)
            {
                SpawnEnemy(selection.data, bestPoint);
                currentBudget -= selection.cost;
            }

            // 3. Pacing: Wait between spawns so it's a stream, not a "clump"
            // High-cost enemies (Demons) create a longer pause
            float delay = selection.cost > 5 ? 3f : Random.Range(0.5f, 1.5f);
            yield return new WaitForSeconds(delay);
        }
    }

    private Transform GetFairSpawnPoint()
    {
        // Use your static list!
        var points = SpawnPoint.SpawnPointList;
        if (points.Count == 0) return null;

        // "Fairness" Filtering:
        // Attempt to find a point that is NOT directly behind the player 
        // and is between 10m and 30m away.
        var fairPoints = points.Where(p =>
        {
            Vector3 directionToPoint = (p.transform.position - playerTransform.position).normalized;
            float dot = Vector3.Dot(playerTransform.forward, directionToPoint);
            float dist = Vector3.Distance(p.transform.position, playerTransform.position);

            // dot > -0.5 means it's not directly behind (roughly 240 degree arc)
            return dot > -0.5f && dist > 8f;
        }).ToList();

        // Fallback: If no "fair" point exists, just take any random one
        var finalPoint = fairPoints.Count > 0 ? fairPoints[Random.Range(0, fairPoints.Count)] : points[Random.Range(0, points.Count)];
        return finalPoint.transform;
    }

    private void SpawnEnemy(EnemyDataSO data, Transform spot)
    {

        Vector2 randomOffset = Random.insideUnitCircle * randomSpawnOffset;
        Vector3 spawnPosition = spot.position + new Vector3(randomOffset.x, 0, randomOffset.y);
        Enemy enemy = Instantiate(enemyPrefab, spot.position, spot.rotation);
        Instantiate(data.GetRandomVisual(), enemy.transform);
        enemy.Initialize(data);

    }
}

[System.Serializable]
public class EnemySpawnConfig
{
    public EnemyDataSO data;
    public int cost;
}