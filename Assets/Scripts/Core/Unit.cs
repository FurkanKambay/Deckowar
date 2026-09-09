using System;
using System.Diagnostics.CodeAnalysis;
using FK.Deckowar.Data;
using UnityEngine;

namespace FK.Deckowar.Core
{
    [Serializable]
    public sealed class Unit
    {
        [field: SerializeField] public UnitAsset UnitAsset { get; private set; }
        [field: SerializeField] public Faction Faction { get; private set; }

        public Vitality Vitality { get; private set; }
        public Intent Intent { get; private set; }

        public Unit(UnitAsset unitAsset, Faction faction)
        {
            UnitAsset = unitAsset ? unitAsset : throw new ArgumentNullException(nameof(unitAsset));
            Vitality = new Vitality(unitAsset.MaxHealth);
            Faction = faction;
        }

        public static implicit operator bool([MaybeNullWhen(false), NotNullWhen(true)] Unit self)
        {
            return self != null && self.UnitAsset && self.Faction is not Faction.None;
        }
    }
}
