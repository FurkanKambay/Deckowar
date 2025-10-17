using UnityEngine;

namespace Deckowar.Data
{
    [CreateAssetMenu]
    public class UnitSO : ScriptableObject
    {
        [Header("Spawn")]
        [SerializeField] private float spawnDelay;

        [Header("Attack")]
        [SerializeField, Min(0)] private int damage = 1;
        [SerializeField, Min(0)] private float attackRange = 1f;
        [SerializeField, Min(0)] private float attackDelay = 1f;

        [Header("Visual")]
        [SerializeField] private Sprite sprite;

        public float SpawnDelay => spawnDelay;
        public int Damage => damage;
        public float AttackRange => attackRange;
        public float AttackDelay => attackDelay;

        public Sprite Sprite => sprite;
    }
}
