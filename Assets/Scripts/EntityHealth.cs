using System;
using UnityEngine;

namespace FurkanKambay
{
    [SelectionBase]
    internal sealed class EntityHealth : MonoBehaviour
    {
        public event Action OnDamageTaken;
        public event Action OnDied;

        [Header("Config")]
        [SerializeField, Min(0)] private int initialHealth;

        public int Health { get; private set; }

        private void Awake()
        {
            Health = initialHealth;
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0)
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
