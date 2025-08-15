using System;
using UnityEngine;

namespace FurkanKambay
{
    [SelectionBase]
    public sealed class Vitality : MonoBehaviour
    {
        public event Action OnDamageTaken;
        public event Action OnDied;

        [Header("Config")]
        [SerializeField, Min(0)] private int initialHealth;

        public int Health
        {
            get => health;
            private set => health = Mathf.Clamp(value, 0, initialHealth);
        }

        private int health;

        private void Awake()
        {
            Health = initialHealth;
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0 || health <= 0)
                return;

            Health -= amount;
            OnDamageTaken?.Invoke();

            MaybeDie();
        }

        private void MaybeDie()
        {
            if (Health > 0)
                return;

            Debug.Log($"{name} died!");
            OnDied?.Invoke();
        }
    }
}
