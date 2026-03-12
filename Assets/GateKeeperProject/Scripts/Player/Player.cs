using System;
using UnityEngine;

namespace GateKeeperProject.Scripts
{
    [RequireComponent(typeof(Health))]
    public class Player : MonoBehaviour
    {
        public Health PlayerHealth { get; private set; }

        private void OnTriggerEnter(Collider col)
        {
            PlayerHealth.TryToTakeDamage(col);
        }

        private void Awake()
        {
            PlayerHealth = GetComponent<Health>();
            PlayerHealth.OnDamageTaken += DamageTakenEvent;
            PlayerHealth.OnDeath += DeathEvent;
            PlayerHealth.SetCurrentToMaxHealth();
        }

        private void OnDestroy()
        {
            PlayerHealth.OnDamageTaken -= DamageTakenEvent;
            PlayerHealth.OnDeath -= DeathEvent;
        }

        private void DamageTakenEvent(float currentHealth)
        {
            Debug.Log("CurrentHealth:" + currentHealth);
        }

        private void DeathEvent()
        {
            gameObject.SetActive(false);
        }


        public void IncreaseMaxHealth()
        {
            float healthMultiplier = 1.1f;
            PlayerHealth.InitAndSetMaxHealth(PlayerHealth.MaxHealth * healthMultiplier);
        }
    }
}