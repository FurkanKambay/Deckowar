using Deckowar.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace Deckowar
{
    public sealed class Unit : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private UnitSO unitSO;
        [SerializeField] private bool shouldMoveRight;

        public UnitSO UnitSO => unitSO;

        public bool CanMove { get; internal set; }
        internal Vector2 MoveDirection { get; private set; }

        private void Awake()
        {
            CanMove = true;
            MoveDirection = shouldMoveRight ? Vector2.right : Vector2.left;
        }

        public void Initialize(UnitSO data)
        {
            unitSO = data;
        }

        private void Start()
        {
            Assert.IsNotNull(unitSO);
        }
    }
}
