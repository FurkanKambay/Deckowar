using UnityEngine;

namespace FurkanKambay
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private bool  shouldMoveRight;

        private void Update()
        {
            Vector2 direction = shouldMoveRight ? Vector2.right : Vector2.left;
            transform.Translate(Time.deltaTime * moveSpeed * direction);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Destroy(gameObject);
        }
    }
}
