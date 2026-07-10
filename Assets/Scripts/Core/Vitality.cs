using System;
using UnityEngine;

namespace FK.Deckowar.Core
{
    public class Vitality
    {
        public event Action OnDamageTaken;
        public event Action OnDied;

        public int MaxHealth { get; private set; }

        public int Health
        {
            get => health;
            private set => health = Mathf.Clamp(value, 0, MaxHealth);
        }

        public bool IsAlive => health > 0;
        public float HealthNormalized => Mathf.InverseLerp(0, MaxHealth, health);

        private int health;

        public Vitality(int maxHealth)
        {
            MaxHealth = maxHealth;
            health = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0 || health <= 0)
                return;

            Health -= amount;
            OnDamageTaken?.Invoke();

            TryDie();
        }

        private void TryDie()
        {
            if (Health <= 0)
                OnDied?.Invoke();
        }
    }
}
