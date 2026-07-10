using UnityEngine;

namespace FK.Deckowar.Data
{
    [CreateAssetMenu]
    public class UnitAsset : ScriptableObject
    {
        [Header("Spawn")]
        [SerializeField, Min(0)] private int spawnDelay;
        [SerializeField, Min(0)] private int maxHealth = 1;

        [Header("Attack")]
        [SerializeField, Min(0)] private int damage = 1;
        [SerializeField, Min(1)] private int attackRange = 1;

        [Header("Visual")]
        [SerializeField] private Sprite sprite;

        public int SpawnDelay => spawnDelay;
        public int MaxHealth => maxHealth;

        public int Damage => damage;
        public int AttackRange => attackRange;

        public Sprite Sprite => sprite;
    }
}
