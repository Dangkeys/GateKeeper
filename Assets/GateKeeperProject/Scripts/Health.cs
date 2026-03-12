using System;
using UnityEngine;

namespace GateKeeperProject.Scripts
{
    public class Health : MonoBehaviour, IDamageable
    {
        [field: SerializeField] public float MaxHealth { get; private set; } = 100;
        [field: SerializeField] public float CurrentHealth { get; private set; }

        [field: SerializeField] public bool IsAlive { get; private set; } = true;
        [field: SerializeField] public float DamageTakenMultiplier { get; private set; } = 1;

        public event Action<float> OnDamageTaken;
        public event Action OnDeath;

        

        public void SetCurrentToMaxHealth()
        {
            CurrentHealth = MaxHealth;
        }

        public void SetMaxHealth(float value)
        {
            MaxHealth = value;
        }

        public void InitAndSetMaxHealth(float value)
        {
            SetMaxHealth(value);
            SetCurrentToMaxHealth();
        }

        public void TakeDamage(float damage)
        {
            if (!IsAlive) return;   
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);
            OnDamageTaken?.Invoke(CurrentHealth);
            IsAlive = CurrentHealth > 0;
            if (IsAlive) return;
            OnDeath?.Invoke();
        }


        public void TryToTakeDamage(Collider col)
        {
            IAttackable attackable = col.GetComponentInParent<IAttackable>();
            
            if (attackable == null) return;
            attackable.Attack(this);
        }

        public void Dead()
        {
            TakeDamage(CurrentHealth);
        }
    }
}