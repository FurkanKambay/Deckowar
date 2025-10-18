using System;
using Deckowar.Core;
using UnityEngine;

namespace Deckowar
{
    public sealed class Attacker : MonoBehaviour
    {
        public event Action OnAttackStarted;
        public event Action OnAttackProcced;

        [Header("References")]
        [SerializeField] private Unit unit;
        [SerializeField] private Vitality vitality;

        [Header("Config")]
        [SerializeField] private LayerMask attackLayers;

        public Intent Intent { get; private set; }

        public void TryAttack()
        {
            if (Intent is Intent.Attack)
                return;

            OnAttackStarted?.Invoke();
            Intent = Intent.Attack;
        }

        internal void ProcAttack()
        {
            Intent = Intent.None;
            OnAttackProcced?.Invoke();
        }
    }
}
