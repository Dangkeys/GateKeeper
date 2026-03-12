using System;
using UnityEngine;

namespace GateKeeperProject.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Health health;

        private void OnTriggerEnter(Collider col)
        {
            health.TryToTakeDamage(col);
        }

        private void Awake()
        {
            health.OnDamageTaken += DamageTakenEvent;
            health.OnDeath += DeathEvent;
            health.Initialize();
        }

        private void OnDestroy()
        {
            health.OnDamageTaken -= DamageTakenEvent;
            health.OnDeath -= DeathEvent;
        }

        private void DamageTakenEvent(float currentHealth)
        {
            Debug.Log("CurrentHealth:" + currentHealth);
        }

        private void DeathEvent()
        {
            // gameObject.SetActive(false);
        }
    }
}