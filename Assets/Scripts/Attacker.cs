using System;
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

        public Unit Unit => unit;
        public bool HasTarget => (bool)target;

        public Vitality Target
        {
            get => target;
            private set
            {
                if (!ReferenceEquals(target, value))
                    target = value;
            }
        }

        private Vitality target;
        private float attackTimer;
        private bool isAttacking;

        private readonly RaycastHit2D[] hits = new RaycastHit2D[1];

        private void Update()
        {
            attackTimer += Time.deltaTime;
        }

        private void FixedUpdate()
        {
            // TryAttack();
        }

        internal void ProcAttack()
        {
            isAttacking = false;
            attackTimer = 0;

            if (!Target)
                return;

            Target.TakeDamage(unit.UnitSO.Damage);
            OnAttackProcced?.Invoke();
        }

        private void TryAttack()
        {
            // if (isAttacking || attackTimer < unit.UnitSO.AttackDelay)
            //     return;

            OnAttackStarted?.Invoke();
            isAttacking = true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(unit.transform.position, unit.MoveDirection * unit.UnitSO.AttackRange);
        }
    }
}
