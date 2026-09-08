using System;
using FK.Deckowar.Data;

namespace FK.Deckowar.Core
{
    public sealed class Unit
    {
        public UnitAsset UnitAsset { get; private set; }
        public Intent Intent { get; private set; }

        public Faction Faction { get; private set; }
        public Vitality Vitality { get; private set; }

        public Unit(UnitAsset unitAsset, Faction faction)
        {
            UnitAsset = unitAsset ? unitAsset : throw new ArgumentNullException(nameof(unitAsset));
            Vitality = new Vitality(unitAsset.MaxHealth);
            Faction = faction;
        }
    }
}
