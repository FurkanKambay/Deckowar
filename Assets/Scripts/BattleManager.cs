using System;
using Deckowar.Core;
using Deckowar.Data;
using UnityEngine;

namespace Deckowar
{
    public sealed class BattleManager : MonoBehaviour
    {
        public event Action<BattleManager, Heading, Unit> OnUnitSpawned;

        [Header("References")]
        [SerializeField] private Battlefield battlefield;

        public void SpawnEastward(UnitSO unitSO) => TrySpawn(Heading.East, unitSO);
        public void SpawnWestward(UnitSO unitSO) => TrySpawn(Heading.West, unitSO);

        private bool TrySpawn(Heading heading, UnitSO unitSO)
        {
            if (!battlefield.CanPushUnit(heading))
                return false;

            var spawnedUnit = new Unit(unitSO);
            bool success = battlefield.PushUnit(heading, spawnedUnit);

            if (success)
                OnUnitSpawned?.Invoke(this, heading, spawnedUnit);

            return success;
        }
    }
}
