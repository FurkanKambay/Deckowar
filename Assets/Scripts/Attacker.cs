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
        [SerializeField] private Vitality   vitality;
        [SerializeField] private Collider2D selfCollider;

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

                target = value;
            }
        }

        private Vitality target;
        private float    attackTimer;
        private bool     isAttacking;

        private RaycastHit2D[] hits = new RaycastHit2D[1];

        private void Update()
        {
            attackTimer += Time.deltaTime;
        }

        private void FixedUpdate()
        {
            int hitCount = selfCollider.Raycast(unit.MoveDirection, hits, attackRange, attackLayers);

            if (hitCount == 0 || !hits[0].collider.TryGetComponent(out Vitality hitTarget))
            {
                Target       = null;
                unit.CanMove = true;
                return;
            }

            unit.CanMove = false;

            if (vitality.Faction == hitTarget.Faction)
            {
                Target = null;
                return;
            }

            Target = hitTarget;

            TryAttack();
        }

        internal void ProcAttack()
        {
            isAttacking = false;
            attackTimer = 0;

            if (!Target)
                return;

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
