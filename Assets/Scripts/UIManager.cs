using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MustafaNaqvi
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Image healthFill;
        [SerializeField] private Image manaFill;
        [SerializeField] private TMP_Text playerProgressText;

        private PlayerController _playerController;

        private void Start()
        {
            _playerController ??= FindObjectOfType<PlayerController>();

            playerProgressText.text = $"\u2022 Collect Key";

            KeyPickup.KeyCollected += () => playerProgressText.text = $"\u2022 Go To Door";
        }

        private void LateUpdate()
        {
            UpdateHealth();
            UpdateMana();
        }

        private void UpdateHealth()
        {
            if (ReferenceEquals(_playerController, null)) return;
            if (!_playerController.TryGetComponent<CharacterHealth>(out var playerHealth)) return;

            healthFill.fillAmount = playerHealth.health / playerHealth.MaxHealth;
        }

        private void UpdateMana()
        {
            if (ReferenceEquals(_playerController, null)) return;
            if (!_playerController.TryGetComponent<PlayerMana>(out var playerMana)) return;

            manaFill.fillAmount = playerMana.Mana / playerMana.MaxMana;
        }
    }
}