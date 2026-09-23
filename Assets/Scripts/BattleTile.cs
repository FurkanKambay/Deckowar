using FK.Deckowar.Core;
using UnityEngine;
using Vertx.Attributes;

namespace FK.Deckowar
{
    public class BattleTile : MonoBehaviour
    {
        [SerializeField, ReadOnlyField] private int rank;
        [SerializeField] private Unit unit;
        // TODO: effects on the tile like `onFire`, etc.

        public int Rank => rank;
        public Unit Unit => unit;
        public bool IsEmpty => !unit;

        internal void Init(int rank)
        {
            this.rank = rank;
        }

        public void SetUnit(Unit unit) => this.unit = unit;
        public void ClearUnit() => this.unit = null;
    }
}
