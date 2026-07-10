using System;
using FK.Common;
using FK.Deckowar.Core;
using FK.Deckowar.Data;
using UnityEngine;

namespace FK.Deckowar
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

        public void SpawnEastward(UnitAsset unitAsset) => TrySpawn(Heading.East, unitAsset);
        public void SpawnWestward(UnitAsset unitAsset) => TrySpawn(Heading.West, unitAsset);

        private bool TrySpawn(Heading heading, UnitAsset unitAsset)
        {
            if (!battlefield.CanPushUnit(heading))
                return false;

            var spawnedUnit = new Unit(unitAsset);
            bool success = battlefield.PushUnit(heading, spawnedUnit);

            if (success)
                OnUnitSpawned?.Invoke(this, heading, spawnedUnit);

            return success;
        }

        private void TurnTimeManager_TurnChanged(TurnTimeManager sender)
        {
            battlefield.AdvanceAllUnits();
        }

        private void Castle_UnitEnqueued(Castle castle, UnitAsset unitAsset)
        {
            Log.Info($"{castle.Faction} Castle queued up {unitAsset.name}", this);
        }

        private void Castle_UnitDequeued(Castle castle, UnitAsset unitAsset)
        {
            Heading heading = castle.Faction.GetHeading();

            TrySpawn(heading, unitAsset);
            Log.Info($"{heading} Castle spawned {unitAsset.name}", this);
        }
    }
}
