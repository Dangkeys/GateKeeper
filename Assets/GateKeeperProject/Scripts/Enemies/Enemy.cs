using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    private NavMeshAgent agent;
    private EnemyStatSO currentStat;
    private BehaviorGraphAgent behaviorGraphAgent;
    private const string AnimatorVariable = "animator";
    private const string MoveSpeedVariable = "moveSpeed";
    private const string DistanceThresholdVariable = "distanceThreshold";
    private const string AnimatorSpeedVariable = "animatorSpeedParam";

    private GameObject enemyVisual;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Initialize(EnemyStatSO stat)
    {
        behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
        currentStat = stat;
        
        enemyVisual = Instantiate(currentStat.GetRandomVisual(), gameObject.transform);
        
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
            
            const int xAxisDirection = 0;
            
            colliderGO.transform.localPosition = enemySizeConfig.Position;
            collider.isTrigger = enemySizeConfig.IsTrigger;
            collider.radius = enemySizeConfig.Radius;
            collider.height = enemySizeConfig.Height;
            collider.center = enemySizeConfig.Center;
            collider.direction = xAxisDirection;
            colliderGO.SetActive(!enemySizeConfig.ShouldDisableOnAwake);
        }
    }

    private void InitializeBehaviorGraphAgent()
    {
        
        behaviorGraphAgent.SetVariableValue(AnimatorVariable, GetComponentInChildren<Animator>());
        behaviorGraphAgent.SetVariableValue(MoveSpeedVariable, currentStat.MoveSpeed);
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

    private void OnTriggerEnter(Collider other)
    {
        OnTryAttackTarget(other.transform);
    }

    private void OnTryAttackTarget(Transform target)
    {
        if (target.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(currentStat.DealDamageAmount);
        }
    }

    public void Die()
    {
        agent.enabled = false;
        gameObject.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        throw new System.NotImplementedException();
    }
}