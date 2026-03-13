using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace GateKeeperProject.Scripts.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private Image healthImage;
        [SerializeField] private float maxRedAlphaColor = 1f;
        [SerializeField] private Health PlayerHealth;


        private void Start()
        {
            PlayerHealth.OnDamageTaken += UpdateHealthUI;
        }

        private void OnDestroy()
        {
            PlayerHealth.OnDamageTaken -= UpdateHealthUI;
        }

        private void UpdateHealthUI(float damageAmount) // damageAmount is unused here, which is fine
        {

            float current = PlayerHealth.CurrentHealth;
            float max = PlayerHealth.MaxHealth;
        
            float damagePercentage = Mathf.Clamp01(1f - (current / max));

            Color tempColor = healthImage.color;
            tempColor.a = maxRedAlphaColor * damagePercentage;
            healthImage.color = tempColor;
        }
    }
}