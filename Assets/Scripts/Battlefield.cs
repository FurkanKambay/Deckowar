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
        [SerializeField, Range(2, 10)] private int rankCount = 5;

        [Header("Debug")]
        [SerializeField, ReadOnlyField, Inline] private Unit[] units;

        public int RankCount => rankCount;

        private void Awake()
        {
            units = new Unit[rankCount];
        }

#region Advancing Units
        public void AdvanceAllUnits()
        {
            Log.Info("Advancing all units", this);
            AdvanceUnits(Faction.Player);
            AdvanceUnits(Faction.Enemy);
        }

        private void AdvanceUnits(Faction faction)
        {
            for (int rank = units.Length; rank >= 0; rank--)
                AdvanceUnit(faction, rank);
        }

        private bool AdvanceUnit(Faction faction, int rank)
        {
            if (rank < 0 || rank >= units.Length)
                return false;

            Unit unit = GetUnitAtRank(faction, rank);
            if (unit is null) return false;

            Unit blockingUnit = GetUnitInFront(faction, rank);
            if (blockingUnit is not null)
                return false;

            bool moveSuccess = SetUnitAtRank(faction, rank + 1, unit);
            if (!moveSuccess) return false;

            bool success = RemoveUnitAtRank(faction, rank);
            return success;
        }
#endregion

        public bool CanPushUnit(Faction faction)
        {
            Unit unit = GetUnitAtRank(faction, 0);
            return unit == null;
        }

        public bool PushUnit(Faction faction, Unit unit)
        {
            if (!CanPushUnit(faction))
                return false;
            return SetUnitAtRank(faction, 0, unit);
        }

        private Unit GetUnitInFront(Faction faction, int rank) =>
            GetUnitAtRank(faction, rank + 1);

        private Unit GetUnitAtRank(Faction faction, int rank)
        {
            if (faction is Faction.None || rank < 0 || rank >= units.Length)
                return null;

            Index index = faction is Faction.Player ? rank : ^(rank + 1);
            return units[index];
        }

        private bool RemoveUnitAtRank(Faction faction, int rank) =>
            SetUnitAtRank(faction, rank, null);

        private bool SetUnitAtRank(Faction faction, int rank, Unit unit)
        {
            if (faction is Faction.None || rank < 0 || rank >= units.Length)
                return false;

            Index index = faction is Faction.Player ? rank : ^(rank + 1);
            units[index] = unit;
            return true;
        }
    }
}
