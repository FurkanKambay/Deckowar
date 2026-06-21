using Deckowar.Common;
using Deckowar.Core;
using UnityEngine;

namespace Deckowar
{
    public sealed class Battlefield : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField, Range(2, 10)] private int cellCount = 5;

        public int CellCount => cellCount;

        private Unit[,] units;
        private int lastNavigableCell;

        private void Awake()
        {
            units = new Unit[2, cellCount - 1]; // 1 unnavigable cell on each side
            //   2          3          4          5
            // -----     -------   --------- -----------
            // [0  ]     [0 1  ]   [0 1 2  ] [0 1 2 3  ]
            // [  0]     [  1 0]   [  2 1 0] [  3 2 1 0]

            lastNavigableCell = cellCount - 2;
        }

        public Unit GetUnit(Heading heading, int cell)
        {
            if (cell < 0 || cell > lastNavigableCell || heading is Heading.None)
                return null;

            return units[GetHeadingIndex(heading), cell];
        }

        public Unit GetOpponent(Heading heading, int cell) =>
            GetUnit(heading.GetOpposite(), lastNavigableCell - cell);

        public bool CanPushUnit(Heading heading) =>
            heading is not Heading.None && GetUnit(heading, 0) == null;

        public bool PushUnit(Heading heading, Unit unit)
        {
            if (!CanPushUnit(heading))
                return false;

            SetUnit(heading, 0, unit);
            return true;
        }

        public bool RemoveUnit(Heading heading, int cell) =>
            SetUnit(heading, cell, null);

        public void AdvanceAllUnits()
        {
            AdvanceUnits(Heading.East);
            AdvanceUnits(Heading.West);
        }

        private void AdvanceUnits(Heading heading)
        {
            EditorDebug.Log($"Advancing {heading}ward units", this);

            for (int cell = lastNavigableCell; cell >= 0; cell--)
                AdvanceUnit(heading, cell);
        }

        public bool AdvanceUnit(Heading heading, int cell)
        {
            if (cell + 1 > lastNavigableCell)
                return false;

            Unit unit = GetUnit(heading, cell);
            Unit blockingUnit = GetUnit(heading, cell + 1);
            Unit opponent = GetOpponent(heading, cell);

            if (unit is null || blockingUnit is not null || opponent is not null)
                return false;

            SetUnit(heading, cell + 1, unit);
            return RemoveUnit(heading, cell);
        }

        private bool SetUnit(Heading heading, int cell, Unit unit)
        {
            if (cell < 0 || cell >= cellCount || heading is Heading.None)
                return false;

            units[GetHeadingIndex(heading), cell] = unit;
            return true;
        }

        private static int GetHeadingIndex(Heading heading) =>
            (int)heading - 1;
    }
}
