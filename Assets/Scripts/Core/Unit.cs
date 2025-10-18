using System;
using Deckowar.Data;

namespace Deckowar.Core
{
    public sealed class Unit
    {
        public UnitSO UnitSO { get; private set; }
        public Intent Intent { get; private set; }

        public Unit(UnitSO unitSO)
        {
            UnitSO = unitSO ? unitSO : throw new ArgumentNullException(nameof(unitSO));
        }
    }
}
