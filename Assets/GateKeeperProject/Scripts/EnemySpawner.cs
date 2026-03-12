using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;
    public float spawnRange = 50f;
    public float spawnInterval = 0.05f; // Rapid spawn
    public int maxEnemies = 10000; // Optional: limit for stress test

    public static int SpawnedCount { get; private set; } = 0;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        while (timer >= spawnInterval && (maxEnemies <= 0 || SpawnedCount < maxEnemies))
        {
            SpawnEnemy();
            timer -= spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(-spawnRange, spawnRange),
            0f,
            Random.Range(-spawnRange, spawnRange)
        );
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        SpawnedCount++;
    }
}
