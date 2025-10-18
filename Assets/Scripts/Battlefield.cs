using Deckowar.Core;
using UnityEngine;

namespace Deckowar
{
    public sealed class Battlefield : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField, Range(2, 10)] private int cellCount = 5;

        private Unit[,] units;
        private int lastNavigableCell;

        private void Awake()
        {
            units = new Unit[2, cellCount - 1]; // 1 unnavigable cell on each side
            //   2          3          4          5
            // -----     -------   --------- -----------
            // [0  ]     [0 1  ]   [0 1 2  ] [0 1 2 3  ]
            // [  0]     [  1 0]   [  2 1 0] [  3 2 1 0]

            lastNavigableCell = units.Length - 1;
        }

        public Unit GetUnit(Heading heading, int cell)
        {
            if (cell < 0 || cell > lastNavigableCell || heading is Heading.None)
                return null;

            return units[(int)heading, cell];
        }

        public Unit GetOpponent(Heading heading, int cell) =>
            GetUnit(heading.GetOpposite(), lastNavigableCell - cell);

        public bool RemoveUnit(Heading heading, int cell) =>
            SetUnit(heading, cell, null);

        public bool MoveUnit(Heading heading, int cell)
        {
            if (cell + 1 > lastNavigableCell)
                return false;

            Unit unit = GetUnit(heading, cell);
            Unit blockingUnit = GetUnit(heading, cell + 1);

            if (!unit || blockingUnit)
                return false;

            SetUnit(heading, cell + 1, unit);
            return RemoveUnit(heading, cell);
        }

        private bool SetUnit(Heading heading, int cell, Unit unit)
        {
            if (cell < 0 || cell >= cellCount || heading is Heading.None)
                return false;

            units[(int)heading, cell] = unit;
            return true;
        }
    }
}
