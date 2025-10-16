using System;
using Deckowar.Core;
using Deckowar.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace Deckowar
{
    public sealed class Unit : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private UnitSO unitSO;
        [SerializeField] private Direction moveDirection;

        public UnitSO UnitSO => unitSO;

        public bool CanMove { get; internal set; }
        internal Vector2 MoveDirection { get; private set; }

        private void Awake()
        {
            CanMove = true;

            MoveDirection = moveDirection switch
            {
                Direction.West => Vector2.left,
                Direction.East => Vector2.right,
                _ => Vector2.zero
            };
        }

        public void Initialize(UnitSO data) =>
            unitSO = data ? data : throw new ArgumentNullException(nameof(data));

        private void Start() =>
            Assert.IsNotNull(unitSO);
    }
}
