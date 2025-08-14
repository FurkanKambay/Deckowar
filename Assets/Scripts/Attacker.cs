using UnityEngine;

namespace FurkanKambay
{
    public sealed class Attacker : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Unit unit;

        [Header("Config")]
        [SerializeField] private LayerMask attackLayers;
        [SerializeField, Min(0)] private int   damage      = 1;
        [SerializeField, Min(0)] private float attackRange = 1f;
        [SerializeField, Min(0)] private float attackDelay = 1f;

        public Vitality Target
        {
            get => target;
            private set
            {
                target       = value;
                unit.CanMove = !value;
            }
        }

        public bool HasTarget => (bool)Target;

        private Vitality target;
        private float    attackTimer;

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
                Debug.Log($"{name} has no target");
                Target = null;
                return;
            }

            if (!hit.collider.TryGetComponent(out Vitality hitTarget))
                return;

            Debug.Log($"{name} targeting {hit.collider.name}");

            Target = hitTarget;
            TryAttack();
        }

        private void TryAttack()
        {
            if (attackTimer < attackDelay)
                return;

            attackTimer = 0;
            Target.TakeDamage(damage);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(unit.Body.position, unit.MoveDirection * attackRange);
        }
    }
}
