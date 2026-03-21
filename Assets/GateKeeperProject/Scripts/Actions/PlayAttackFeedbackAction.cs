using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlayAttackFeedback", story: "[Enemy] Play attack MMfeedback", category: "Action", id: "8e8fdc54ebb3a7fa5ad315c0c0ca4bc6")]
public partial class PlayAttackFeedbackAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Enemy;

    protected override Status OnStart()
    {
        Enemy.Value.OnAttack();
        return Status.Success;
    }


}

