using System;
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
    }
}
