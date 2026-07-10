using System;
using FK.Deckowar.Data;

namespace FK.Deckowar.Core
{
    public sealed class Unit
    {
        public UnitSO UnitSO { get; private set; }
        public Intent Intent { get; private set; }

        public Faction Faction { get; private set; }
        public Vitality Vitality { get; private set; }

        public Unit(UnitSO unitSO)
        {
            UnitSO = unitSO ? unitSO : throw new ArgumentNullException(nameof(unitSO));
            Vitality = new Vitality(unitSO.MaxHealth);
        }
    }
}
