using System;
using FK.Common;
using FK.Deckowar.Core;
using UnityEngine;
using Vertx.Attributes;

namespace FK.Deckowar
{
    public sealed class Battlefield : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField, Range(2, 10)] private int cellCount = 5;

        [Header("Debug")]
        [SerializeField, ReadOnlyField, Inline] private Unit[] units;

        public int CellCount => cellCount;

        private void Awake()
        {
            units = new Unit[cellCount];
        }

#region Advancing Units
        public void AdvanceAllUnits()
        {
            Log.Info("Advancing all units", this);
            AdvanceUnits(Heading.East);
            AdvanceUnits(Heading.West);
        }

        private void AdvanceUnits(Heading heading)
        {
            for (int cell = units.Length; cell >= 0; cell--)
                AdvanceUnit(heading, cell);
        }

        private bool AdvanceUnit(Heading heading, int cell)
        {
            if (cell < 0 || cell >= units.Length)
                return false;

            Unit unit = GetUnitAtCell(heading, cell);
            if (unit is null) return false;

            Unit blockingUnit = GetUnitInFront(heading, cell);
            if (blockingUnit is not null)
                return false;

            bool moveSuccess = SetUnitAtCell(heading, cell + 1, unit);
            if (!moveSuccess) return false;

            bool success = SetUnitAtCell(heading, cell, null);
            return success;
        }
#endregion

        public bool CanPushUnit(Heading heading)
        {
            Unit unit = GetUnitAtCell(heading, 0);
            return unit == null;
        }

        public bool PushUnit(Heading heading, Unit unit)
        {
            if (!CanPushUnit(heading))
                return false;
            return SetUnitAtCell(heading, 0, unit);
        }

        private Unit GetUnitInFront(Heading heading, int cell) =>
            GetUnitAtCell(heading, cell + 1);

        private Unit GetUnitAtCell(Heading heading, int cell)
        {
            if (heading is Heading.None || cell < 0 || cell >= units.Length)
                return null;

            Index index = heading is Heading.East ? cell : ^(cell + 1);
            return units[index];
        }

        private bool RemoveUnitAtCell(Heading heading, int cell) =>
            SetUnitAtCell(heading, cell, null);

        private bool SetUnitAtCell(Heading heading, int cell, Unit unit)
        {
            if (heading is Heading.None || cell < 0 || cell >= units.Length)
                return false;

            Index index = heading is Heading.East ? cell : ^(cell + 1);
            units[index] = unit;
            return true;
        }
    }
}
