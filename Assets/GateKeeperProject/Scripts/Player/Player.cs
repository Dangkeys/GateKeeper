using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.Feedbacks;
using VContainer;

namespace GateKeeperProject.Scripts
{
    [RequireComponent(typeof(Health))]
    public class Player : MonoBehaviour
    {
        public Health PlayerHealth { get; private set; }
        private SaveManager _saveManager;

        [Header("Feedbacks")]
        [SerializeField] private MMF_Player hurtFeedbacks;

        [Inject]
        public void Construct(SaveManager saveManager)
        {
            _saveManager = saveManager;
        }
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

        private async void DeathEvent()
        {
            SceneManager.LoadScene("Main Menu");
            // 1. Play Fade MMFeedback
            // playFeedback();

            if (_saveManager != null)
            {
                await _saveManager.SaveGameAsync();
            }

            SceneManager.LoadSceneAsync("ScoreScene");
        }

        public void IncreaseMaxHealth()
        {
            float healthMultiplier = 1.1f;
            PlayerHealth.InitAndSetMaxHealth(PlayerHealth.MaxHealth * healthMultiplier);
        }
    }
}