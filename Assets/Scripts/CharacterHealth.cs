using System;
using UnityEngine;

namespace MustafaNaqvi
{
    public class CharacterHealth : MonoBehaviour
    {
        [SerializeField] private float healthMaxValue;

        public float MaxHealth => healthMaxValue;
        public float health;
        public Action death;

        private void Start()
        {
            ResetHealth();
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

        public void AddHealth(float healthToAdd)
        {
            health += healthToAdd;
            if (health <= healthMaxValue) return;
            ResetHealth();
        }

        private void ResetHealth() => health = healthMaxValue;
    }
}