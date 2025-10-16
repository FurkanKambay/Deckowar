using System;
using UnityEngine;

namespace Deckowar
{
    public enum Faction
    {
        Player,
        Enemy
    }

    [SelectionBase]
    public sealed class Vitality : MonoBehaviour
    {
        public event Action OnDamageTaken;
        public event Action OnDied;

        [Header("Config")]
        [SerializeField] private Faction faction;
        [SerializeField, Min(0)] private int maxHealth;

        public Faction Faction => faction;
        public int MaxHealth => maxHealth;
        public bool IsAlive => health > 0;

        public float HealthNormalized => Mathf.InverseLerp(0, maxHealth, health);

        public int Health
        {
            get => health;
            private set => health = Mathf.Clamp(value, 0, maxHealth);
        }

        private int health;

        private void Awake()
        {
            Health = maxHealth;
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
