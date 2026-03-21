using System;
using GateKeeperProject.Scripts.Enemies;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ExecuteEnemyAbility", story: "Execute [AbilityID] on [Agent] targeting [Target]",
    category: "Action", id: "7df44098fda678ca16ad66a5c6b66a02")]
public partial class ExecuteEnemyAbilityAction : Action
{
    [SerializeReference] public BlackboardVariable<string> AbilityID;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    private bool isAbilityFinished = false;

    private IEnemyAbility cachedAbility;
    private bool hasInitialized = false;

    protected override Status OnStart()
    {
        if (Agent.Value == null) return Status.Failure;
        isAbilityFinished = false;
        
        if (!hasInitialized)
        {
            IEnemyAbility[] abilities = Agent.Value.GetComponentsInChildren<IEnemyAbility>();
            foreach (var ability in abilities)
            {
                if (ability.AbilityID == AbilityID.Value)
                {
                    cachedAbility = ability;
                    hasInitialized = true;
                    break;
                }
            }
        }

        if (cachedAbility != null)
        {
            cachedAbility.ExecuteAbility(Target.Value, () => { isAbilityFinished = true; });

            return Status.Running;
        }

        Debug.LogWarning($"หาท่า {AbilityID.Value} ไม่เจอ!");
        return Status.Failure;
    }

    protected override Status OnUpdate()
    {
        // 3. เช็ค Boolean แค่ตัวเดียว กิน CPU น้อยระดับเสี้ยวนาโนวินาที ปลอดภัยมากครับ!
        if (isAbilityFinished)
        {
            return Status.Success;
        }

        return Status.Running;
    }
}