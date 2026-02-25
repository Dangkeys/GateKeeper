using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private EnemyDataSO currentData;

    // Behavior Tree accessible variables
    [HideInInspector] public float currentDamage;
    [HideInInspector] public float currentAttackRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Initialize(EnemyDataSO data)
    {
        currentData = data;


        // 2. Map NavMesh Agent Settings
        if (agent != null)
        {
            // Set the Agent Type (Giant, Mini, Humanoid)
            int? typeID = data.GetNavMeshAgentID();
            if (typeID.HasValue)
            {
                agent.agentTypeID = typeID.Value;
            }
            else
            {
                Debug.LogWarning($"NavMesh Agent Type '{data.Type}' not found in Navigation Settings!");
            }

            // Apply movement stats
            agent.speed = data.MoveSpeed;
            agent.stoppingDistance = data.StoppingDistance;
            agent.angularSpeed = data.RotationSpeed;
        }

        currentDamage = data.DealDamageAmount;
        currentAttackRange = data.AttackRange;

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