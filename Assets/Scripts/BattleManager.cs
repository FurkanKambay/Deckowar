using System;
using Deckowar.Core;
using Deckowar.Data;
using Furkan.Common;
using UnityEngine;

namespace Deckowar
{
    public sealed class BattleManager : MonoBehaviour
    {
        public event Action<BattleManager, Heading, Unit> OnUnitSpawned;

        [Header("References")]
        [SerializeField] private TurnTimeManager turnTimeManager;
        [SerializeField] private Battlefield battlefield;
        [SerializeField] private Castle castlePlayer;
        [SerializeField] private Castle castleEnemy;

#region Unity Callbacks
        private void OnEnable()
        {
            turnTimeManager.OnTurnChanged += TurnTimeManager_TurnChanged;

            castlePlayer.OnUnitEnqueued += Castle_UnitEnqueued;
            castleEnemy.OnUnitEnqueued += Castle_UnitEnqueued;

            castlePlayer.OnUnitDequeued += Castle_UnitDequeued;
            castleEnemy.OnUnitDequeued += Castle_UnitDequeued;
        }

        private void OnDisable()
        {
            turnTimeManager.OnTurnChanged -= TurnTimeManager_TurnChanged;

            castlePlayer.OnUnitEnqueued -= Castle_UnitEnqueued;
            castleEnemy.OnUnitEnqueued -= Castle_UnitEnqueued;

            castlePlayer.OnUnitDequeued -= Castle_UnitDequeued;
            castleEnemy.OnUnitDequeued -= Castle_UnitDequeued;
        }
#endregion

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

        private void TurnTimeManager_TurnChanged(TurnTimeManager sender)
        {
            battlefield.AdvanceAllUnits();
        }

        private void Castle_UnitEnqueued(Castle castle, UnitSO unitSO)
        {
            Log.Info($"{castle.Faction} Castle queued up {unitSO.name}", this);
        }

        private void Castle_UnitDequeued(Castle castle, UnitSO unitSO)
        {
            Heading heading = castle.Faction.GetHeading();

            TrySpawn(heading, unitSO);
            Log.Info($"{heading} Castle spawned {unitSO.name}", this);
        }
    }
}
