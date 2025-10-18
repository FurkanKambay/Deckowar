using System;
using Deckowar.Core;
using Deckowar.Data;

namespace Deckowar
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
