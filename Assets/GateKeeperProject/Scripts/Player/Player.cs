using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.Feedbacks;

namespace GateKeeperProject.Scripts
{
    [RequireComponent(typeof(Health))]
    public class Player : MonoBehaviour
    {
        public Health PlayerHealth { get; private set; }

        [Header("Feedbacks")]
        [SerializeField] private MMF_Player hurtFeedbacks;

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
            hurtFeedbacks?.PlayFeedbacks();
        }

        private void DeathEvent()
        {
            SceneManager.LoadScene("Main Menu");
        }

        public void IncreaseMaxHealth()
        {
            float healthMultiplier = 1.1f;
            PlayerHealth.InitAndSetMaxHealth(PlayerHealth.MaxHealth * healthMultiplier);
        }
    }
}