using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace GateKeeperProject.Scripts.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private Image healthImage;
        [SerializeField] private float maxRedAlphaColor = 1f;
        private Player _player;

        [Inject]
        private void Construct(Player player)
        {
            _player = player;
            _player.PlayerHealth.OnDamageTaken += UpdateHealthUI;

        }

        private void OnDestroy()
        {
            if (_player != null && _player.PlayerHealth != null)
            {
                _player.PlayerHealth.OnDamageTaken -= UpdateHealthUI;
            }
        }

        private void UpdateHealthUI(float damageAmount)
        {
            float maxHealth = _player.PlayerHealth.MaxHealth;
            float healthPercentage = _player.PlayerHealth.CurrentHealth / maxHealth;
            float damagePercentage = 1f - healthPercentage;

            Color tempColor = healthImage.color;
            tempColor.a = maxRedAlphaColor * damagePercentage;
            healthImage.color = tempColor;
        }
    }
}