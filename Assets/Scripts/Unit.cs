using UnityEngine;

namespace FurkanKambay
{
    public sealed class Unit : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D body;

        [Header("Config")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private bool shouldMoveRight;

        private bool         hasTarget;
        private EntityHealth target;
        private Vector2      moveDirection;

        private void Awake()
        {
            moveDirection = shouldMoveRight ? Vector2.right : Vector2.left;
        }

        private void FixedUpdate()
        {
            if (hasTarget)
                return;

            Vector2 moveDelta = Time.deltaTime * moveSpeed * moveDirection;
            body.MovePosition(body.position + moveDelta);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"{name}: OnTriggerEnter2D. other: {other.name}");

            if (!other.TryGetComponent(out EntityHealth health))
                return;

            hasTarget = true;
            target    = health;

            health.TakeDamage(1);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log($"{name}: OnTriggerExit2D. other: {other.name}");
        }
    }
}
