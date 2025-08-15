using System;
using UnityEngine;

namespace FurkanKambay
{
    public sealed class Attacker : MonoBehaviour
    {
        public event Action OnAttackStarted;
        public event Action OnAttackProcced;

        [Header("References")]
        [SerializeField] private Unit unit;

        [Header("Config")]
        [SerializeField] private LayerMask attackLayers;
        [SerializeField, Min(0)] private int   damage      = 1;
        [SerializeField, Min(0)] private float attackRange = 1f;
        [SerializeField, Min(0)] private float attackDelay = 1f;

        public bool HasTarget => (bool)target;

        public Vitality Target
        {
            get => target;
            private set
            {
                if (ReferenceEquals(target, value))
                    return;

                target       = value;
                unit.CanMove = !value;
            }
        }

        private Vitality target;
        private float    attackTimer;
        private bool     isAttacking;

        private void Update()
        {
            attackTimer += Time.deltaTime;
        }

        private void FixedUpdate()
        {
            Vector2 origin    = unit.Body.position;
            Vector2 direction = unit.MoveDirection;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, attackRange, attackLayers);

            if (!hit)
            {
                Target = null;
                return;
            }

            if (!hit.collider.TryGetComponent(out Vitality hitTarget))
                return;

            Target = hitTarget;
            TryAttack();
        }

        internal void ProcAttack()
        {
            isAttacking = false;
            attackTimer = 0;

            Target.TakeDamage(damage);
            OnAttackProcced?.Invoke();
        }

        private void TryAttack()
        {
            if (isAttacking || attackTimer < attackDelay)
                return;

            OnAttackStarted?.Invoke();
            isAttacking = true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(unit.Body.position, unit.MoveDirection * attackRange);
        }
    }
}
