using System;
using System.Diagnostics.CodeAnalysis;
using FK.Deckowar.Core;
using FK.Deckowar.Data;
using UnityEngine;

namespace FK.Deckowar
{
    public sealed class Unit : MonoBehaviour
    {
        [SerializeField] private UnitAsset unitAsset;
        [SerializeField] private Faction faction;

        public UnitAsset UnitAsset => unitAsset;
        public Faction Faction => faction;
        public Vitality Vitality { get; private set; }

        internal void Init(UnitAsset unitAsset, Faction faction)
        {
            this.unitAsset = unitAsset ? unitAsset : throw new ArgumentNullException(nameof(unitAsset));
            this.faction = faction;
            Vitality = new Vitality(unitAsset.MaxHealth);

            name = $"{faction}[{unitAsset.name}]";
        }

        public static implicit operator bool([MaybeNullWhen(false), NotNullWhen(true)] Unit self)
        {
            return self != null && self.UnitAsset && self.Faction is not Faction.None;
        }
    }
}
