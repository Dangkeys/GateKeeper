using System;
using System.Collections.Generic;
using GateKeeperProject.Scripts;
using GateKeeperProject.Scripts.Enemies;
using JetBrains.Annotations;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour, IAttackable
{
    private NavMeshAgent agent;
    private EnemyStatSO currentStat;
    private BehaviorGraphAgent behaviorGraphAgent;
    private const string AnimatorVariable = "animator";
    private const string MoveSpeedVariable = "moveSpeed";
    private const string DistanceThresholdVariable = "distanceThreshold";
    private const string AnimatorSpeedVariable = "animatorSpeedParam";

    private EnemyStatModifiers _enemyStatModifiers;

    public Health EnemyHealth { get; private set; }

    private GameObject enemyVisual;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        EnemyHealth = GetComponent<Health>();
        EnemyHealth.OnDamageTaken += DamageTakenEvent;
        EnemyHealth.OnDeath += DeathEvent;
    }
    private void OnDestroy()
    {
        EnemyHealth.OnDamageTaken -= DamageTakenEvent;
        EnemyHealth.OnDeath -= DeathEvent;
    }
    private void DamageTakenEvent(float currentHealth)
    {
        // Debug.Log(gameObject.name +  " damaged!" + ", CurrentHealth:"  + currentHealth);
    }

    private void DeathEvent()
    {
        agent.enabled = false;
        Destroy(gameObject);
        // gameObject.SetActive(false);
    }

    public void Initialize(EnemyStatSO stat, EnemyStatModifiers  statModifiers)
    {
        behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
        currentStat = stat;
        _enemyStatModifiers = statModifiers;
        
        enemyVisual = Instantiate(currentStat.GetRandomVisual(), gameObject.transform);
        EnemyHealth.InitAndSetMaxHealth(currentStat.MaxHealth * _enemyStatModifiers.healthMultiplier);
        
        InitializeColliders();
        InitializeAgent();
        InitializeBehaviorGraphAgent();

    }
    
    
    private void InitializeAgent()
    {
        // 2. Map NavMesh Agent Settings
        int? typeID = currentStat.GetNavMeshAgentID();
        if (!typeID.HasValue)
        {
            Debug.LogWarning($"NavMesh Agent Type '{currentStat.Type}' not found in Navigation Settings!");
            return;
        }

        agent.agentTypeID = typeID.Value;
        agent.height = currentStat.SizeConfigSo.AgentHeight;
        agent.radius = currentStat.SizeConfigSo.AgentRadius;
        agent.avoidancePriority = UnityEngine.Random.Range(1, 100);
        agent.stoppingDistance = currentStat.StoppingDistance;
    }

    private void InitializeColliders()
    {
        foreach (EnemyColliderConfig enemySizeConfig in currentStat.SizeConfigSo.ColliderConfigList)
        {
            const string colliderGOName = "Collider";
            GameObject colliderGO = new GameObject(colliderGOName);
            colliderGO.tag = enemySizeConfig.GameObjectTag;
            colliderGO.layer = LayerMask.NameToLayer(enemySizeConfig.GameObjectLayer);
            Transform? parentTransform =
                RecursiveFindChild(enemyVisual.transform, enemySizeConfig.ParentTransformNameList).transform;
            if (parentTransform == null)
            {
                Debug.LogError("Parent Transform not found!");
                return;
            }

            colliderGO.transform.SetParent(parentTransform!, worldPositionStays: false);
            CapsuleCollider collider = colliderGO.AddComponent<CapsuleCollider>();
            

            
            colliderGO.transform.localPosition = enemySizeConfig.Position;
            colliderGO.transform.localRotation = Quaternion.Euler(enemySizeConfig.Rotation);
            
            collider.isTrigger = enemySizeConfig.IsTrigger;
            collider.radius = enemySizeConfig.Radius;
            collider.height = enemySizeConfig.Height;
            collider.center = enemySizeConfig.Center;
            collider.direction = (int)enemySizeConfig.EnemyColliderDirection;
            colliderGO.SetActive(!enemySizeConfig.ShouldDisableOnAwake);
        }
    }

    private void InitializeBehaviorGraphAgent()
    {
        
        behaviorGraphAgent.SetVariableValue(AnimatorVariable, GetComponentInChildren<Animator>());
        behaviorGraphAgent.SetVariableValue(MoveSpeedVariable, currentStat.MoveSpeed * _enemyStatModifiers.moveSpeedMultiplier);
        behaviorGraphAgent.SetVariableValue(DistanceThresholdVariable, currentStat.StoppingDistance);
        behaviorGraphAgent.SetVariableValue(AnimatorSpeedVariable, "velocity");
        agent.angularSpeed = currentStat.RotationSpeed;
    }


    [CanBeNull]
    public static GameObject RecursiveFindChild(Transform parent, List<string> childNameList)
    {
        // Optional but recommended: guard clauses to prevent NullReferenceExceptions
        if (!parent || childNameList == null) return null;

        foreach (Transform child in parent)
        {
            // CORRECTED: Use .Contains() instead of 'in'
            if (childNameList.Contains(child.name))
            {
                return child.gameObject;
            }

            // Recursively call for children of the current child
            GameObject found = RecursiveFindChild(child, childNameList);
            if (found != null)
            {
                return found;
            }
        }

        return null;
    }
    

    public void Attack(IDamageable damageable)
    {
        damageable.TakeDamage(currentStat.DealDamageAmount * _enemyStatModifiers.damageMultiplier);
    }


}