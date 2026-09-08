using FK.Deckowar.Core;
using UnityEngine;

namespace FK.Deckowar.Data
{
    [CreateAssetMenu(menuName = "Castle/Stats")]
    public class CastleStatsAsset : ScriptableObject
    {
        [SerializeField] private Faction faction;

        [Header("Stats")]
        [SerializeField, Min(1)] private int maxHealth = 20;
        [SerializeField, Min(0)] private float goldGainPerTurn = 1;

        [Header("Visual")]
        [SerializeField] private Sprite sprite;

        public Faction Faction => faction;
        public int MaxHealth => maxHealth;
        public float GoldGainPerTurn => goldGainPerTurn;
        public Sprite Sprite => sprite;
    }
}
