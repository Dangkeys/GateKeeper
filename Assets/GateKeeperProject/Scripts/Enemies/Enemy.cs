using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private EnemyDataSO currentData;
    private BehaviorGraphAgent behaviorGraphAgent;
    private const string AnimatorVariable = "animator";
    private const string MoveSpeedVariable = "moveSpeed";
    private const string DistanceThresholdVariable = "distanceThreshold";
    private const string AnimatorSpeedVariable = "animatorSpeedParam";
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Initialize(EnemyDataSO data)
    {
        behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
        currentData = data;
        Instantiate(currentData.GetRandomVisual(), gameObject.transform);

        // 2. Map NavMesh Agent Settings
        int? typeID = data.GetNavMeshAgentID();
        if (typeID.HasValue)
        {
            agent.agentTypeID = typeID.Value;
        }
        else
        {
            Debug.LogWarning($"NavMesh Agent Type '{data.Type}' not found in Navigation Settings!");
        }

        behaviorGraphAgent.SetVariableValue(AnimatorVariable, GetComponentInChildren<Animator>());
        behaviorGraphAgent.SetVariableValue(MoveSpeedVariable, data.MoveSpeed);
        behaviorGraphAgent.SetVariableValue(DistanceThresholdVariable, data.StoppingDistance);
        behaviorGraphAgent.SetVariableValue(AnimatorSpeedVariable, "velocity");
        agent.angularSpeed = data.RotationSpeed;




    }

    // public void OnAttackTarget(Transform target)
    // {
    //     if (target.TryGetComponent(out IDamageable damageable))
    //     {
    //         damageable.TakeDamage(currentDamage, target.position);
    //     }
    // }

    // protected override void Die()
    // {
    //     // Disable agent and trigger death logic
    //     agent.enabled = false;
    //     gameObject.SetActive(false);
    // }
}