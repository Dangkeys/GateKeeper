using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EnemyStatSO", menuName = "Scriptable Objects/EnemyStatSO")]
public class EnemyStatSO : ScriptableObject
{
    public enum AgentType
    {
        Giant,
        Mini,
        Humanoid
    }

    [Header("Visuals")]
    [Tooltip("List of possible prefabs for this enemy type to provide variety.")]
    [field: SerializeField] public List<GameObject> EnemyPrefabVisualList { get; private set; }


    [Header("Collider")] [field: FormerlySerializedAs("<ColliderSO>k__BackingField")] [field: SerializeField] public EnemySizeConfigSO SizeConfigSo {get; private set;}

    [Header("Identity")]
    [field: SerializeField] public string Name { get; private set; }

    [Header("Base Stats")]
    [field: SerializeField] public float MaxHealth { get; private set; } = 100f;
    [field: SerializeField] public float DamageReductionRate { get; private set; } = 1f;

    [Header("Movement")]
    [field: SerializeField] public float MoveSpeed { get; private set; } = 3.5f;
    [field: SerializeField] public float StoppingDistance { get; private set; } = 1.5f;
    [field: SerializeField] public float RotationSpeed { get; private set; } = 120f;

    [Header("Combat Settings")]
    [field: SerializeField] public float DealDamageAmount { get; private set; } = 10f;
    [field: SerializeField] public float AttackRange { get; private set; } = 2f;
    [field: SerializeField] public float AttackCooldown { get; private set; } = 1.5f;
     [field: SerializeField] public AgentType Type { get; private set; } = AgentType.Humanoid;

    [Header("Roguelike / Rewards")]
    [field: SerializeField] public int scoreValue { get; private set; } = 100;
    [Range(0, 1)] public float dropChance { get; private set; } = 0.2f;

    /// <summary>
    /// Returns a random prefab from the visual list.
    /// </summary>
    public GameObject GetRandomVisual()
    {
        if (EnemyPrefabVisualList == null || EnemyPrefabVisualList.Count == 0)
        {
            Debug.LogWarning($"EnemyDataSO: {Name} has no prefabs assigned!");
            return null;
        }
        int randomIndex = Random.Range(0, EnemyPrefabVisualList.Count);
        return EnemyPrefabVisualList[randomIndex];
    }
    // Helper to find ID by name, utilizing NavMesh.GetSettingsCount and NavMesh.GetSettingsByIndex
    private int? GetNavMeshAgentID(string name)
    {
        for (int i = 0; i < NavMesh.GetSettingsCount(); i++)
        {
            NavMeshBuildSettings settings = NavMesh.GetSettingsByIndex(i);
            if (name == NavMesh.GetSettingsNameFromID(settings.agentTypeID))
            {
                return settings.agentTypeID;
            }
        }
        return null;
    }

    public int? GetNavMeshAgentID()
    {
        return GetNavMeshAgentID(Type.ToString());
    }
    
    
}
