using System;
using UnityEngine;

namespace GateKeeperProject.Scripts.Enemies
{
    public interface IEnemyAbility
    {
        string AbilityID { get; }
    

        void ExecuteAbility(GameObject target, Action onComplete);
    }
}