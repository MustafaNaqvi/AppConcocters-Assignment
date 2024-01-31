using System;
using UnityEngine;

namespace MustafaNaqvi
{
    public class CharacterHealth : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private float healthMaxValue;

        public float health;
        public Action death;

        private void Start()
        {
            if (ReferenceEquals(playerController, null) && TryGetComponent<PlayerController>(out var pc))
                playerController = pc;

            health = healthMaxValue;
        }

        public void Damage(float damage)
        {
            if (health <= 0f)
            {
                health = 0f;
                return;
            }

            health -= damage;

            if (health > 0f) return;
            death?.Invoke();
            health = 0f;
        }

        private void ResetHealth() => health = healthMaxValue;
    }
}