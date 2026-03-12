using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "WaveSettingsSO", menuName = "Scriptable Objects/WaveSettingsSO")]
public class WaveSettingsSO : ScriptableObject
{
    [Header("Prefabs")] public Enemy enemyPrefab;
    public float randomSpawnOffset = 10f;

    [Header("Enemy Pool & Weights")] public List<EnemySpawnConfig> enemyPool;

    [Header("Wave Pacing & Budget")] public int baseWaveBudget = 20;
    public int budgetIncreasePerWave = 15;
    public int maxWaveBudget = 150;
    public float spawnDelay = 0.5f;

    [Header("Enemy Stat Scaling")] public EnemyWaveConfig waveConfig;
}

[System.Serializable]
public class EnemySpawnConfig
{
    [FormerlySerializedAs("data")] public EnemyStatSO stat;
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
    public float maxHealthMultiplier = 3.0f;
    public float maxDamageMultiplier = 2.5f;
    public float maxMoveSpeedMultiplier = 1.5f;
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