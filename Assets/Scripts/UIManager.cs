using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MustafaNaqvi
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Components")] [SerializeField]
        private Image healthFill;

        [SerializeField] private Image manaFill;
        [SerializeField] private TMP_Text playerProgressText;

        [Header("Panels")] [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject winPanel;

        private PlayerController _playerController;
        private CharacterHealth _playerHealth;
        private PlayerMana _playerMana;

        private void Start()
        {
            _playerController ??= FindObjectOfType<PlayerController>();

            if (ReferenceEquals(_playerController, null)) return;

            _playerController.win += Win;

            if (ReferenceEquals(_playerHealth, null) &&
                _playerController.TryGetComponent<CharacterHealth>(out var playerHealth))
                _playerHealth = playerHealth;

            if (ReferenceEquals(_playerMana, null) &&
                _playerController.TryGetComponent<PlayerMana>(out var playerMana))
                _playerMana = playerMana;

            if (ReferenceEquals(_playerHealth, null)) return;
            _playerHealth.death += GameOver;

            playerProgressText.text = $"\u2022 Collect Key";

            KeyPickup.KeyCollected += () => playerProgressText.text = $"\u2022 Go To Door";
        }

        private void OnDestroy()
        {
            if (ReferenceEquals(_playerController, null)) return;
            _playerController.win -= Win;

            if (ReferenceEquals(_playerHealth, null)) return;
            _playerHealth.death -= GameOver;
        }


        private void LateUpdate()
        {
            UpdateHealth();
            UpdateMana();
        }

        private void UpdateHealth()
        {
            if (ReferenceEquals(_playerHealth, null)) return;

            healthFill.fillAmount = _playerHealth.health / _playerHealth.MaxHealth;
        }

        private void UpdateMana()
        {
            if (ReferenceEquals(_playerMana, null)) return;

            manaFill.fillAmount = _playerMana.Mana / _playerMana.MaxMana;
        }

        private void GameOver()
        {
            Time.timeScale = 0f;
            gameOverPanel.SetActive(true);
        }

        private void Win()
        {
            Time.timeScale = 0f;
            winPanel.SetActive(true);
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}