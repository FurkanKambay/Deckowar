using FurkanKambay.Data;
using UnityEngine;

namespace FurkanKambay
{
    public sealed class Unit : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D body;

        [Header("Config")]
        [SerializeField] private UnitSO unitSO;
        [SerializeField] private bool shouldMoveRight;

        public bool CanMove { get; internal set; }
        internal Vector2 MoveDirection { get; private set; }

        public Rigidbody2D Body => body;
        public UnitSO UnitSO => unitSO;

        private void Awake()
        {
            CanMove = true;
            MoveDirection = shouldMoveRight ? Vector2.right : Vector2.left;
        }

        public void SetData(UnitSO data) =>
            unitSO = data;

        private void FixedUpdate()
        {
            if (!unitSO)
                return;

            if (!CanMove)
            {
                body.linearVelocity = Vector2.zero;
                return;
            }

            Vector2 moveVector = unitSO.MoveSpeed * MoveDirection;
            // body.linearVelocity = moveVector;

            Vector2 moveDelta = Time.deltaTime * moveVector;
            body.MovePosition(body.position + moveDelta);
        }
    }
}
