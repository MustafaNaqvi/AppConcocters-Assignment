using UnityEngine;

namespace MustafaNaqvi
{
    public class PlayerMana : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private float manaRegenerationTime;
        [SerializeField] private float manaMaxValue;

        internal float Mana;

        private void Start()
        {
            if (ReferenceEquals(playerController, null) && TryGetComponent<PlayerController>(out var pc))
                playerController = pc;
        }

        private void Update()
        {
            HandleManaRegeneration();
        }

        private void HandleManaRegeneration()
        {
            if (Mana >= manaMaxValue)
            {
                Mana = manaMaxValue;
                return;
            }

            Mana = Mathf.MoveTowards(Mana, manaMaxValue, Time.deltaTime * manaRegenerationTime);
        }

        public void ConsumeMana(float value)
        {
            if (Mana <= 0f)
            {
                Mana = 0f;
                return;
            }

            Mana -= value;
        }

        public void ResetMana() => Mana = 0f;
    }
}