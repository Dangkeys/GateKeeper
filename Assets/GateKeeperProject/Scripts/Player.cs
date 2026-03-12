using System;
using UnityEngine;

namespace GateKeeperProject.Scripts
{
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
    }
}