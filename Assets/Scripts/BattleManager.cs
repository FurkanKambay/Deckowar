using System;
using FK.Common;
using FK.Deckowar.Core;
using FK.Deckowar.Data;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FK.Deckowar
{
    [Serializable]
    public struct TurnInfo
    {
        public int turnIndex;
        public Faction faction;

        public void NextTurn()
        {
            turnIndex++;
            faction = turnIndex % 2 == 0 ? Faction.Player : Faction.Enemy;
        }
    }

    public sealed class BattleManager : MonoBehaviour
    {
        public event Action<BattleManager, TurnInfo> OnTurnChanged;
        public event Action<BattleManager, Faction, UnitAsset> OnUnitSpawned;

        [Header("Input")]
        [SerializeField] private InputActionReference readyInput;

        [Header("References")]
        [SerializeField] private Battlefield battlefield;
        [SerializeField] private Castle castlePlayer;
        [SerializeField] private Castle castleEnemy;

        [Header("Debug")]
        [SerializeField] private TurnInfo currentTurn;

#region Unity Callbacks
        private void Awake()
        {
            Assert.IsNotNull(readyInput);
            readyInput.asset.Enable();

            currentTurn = new TurnInfo { turnIndex = -1, faction = Faction.None };
        }

        private void OnEnable()
        {
            castlePlayer.OnUnitEnqueued += Castle_UnitEnqueued;
            castlePlayer.OnUnitDequeued += Castle_UnitDequeued;
            castleEnemy.OnUnitEnqueued += Castle_UnitEnqueued;
            castleEnemy.OnUnitDequeued += Castle_UnitDequeued;
        }

        private void OnDisable()
        {
            castlePlayer.OnUnitEnqueued -= Castle_UnitEnqueued;
            castlePlayer.OnUnitDequeued -= Castle_UnitDequeued;
            castleEnemy.OnUnitEnqueued -= Castle_UnitEnqueued;
            castleEnemy.OnUnitDequeued -= Castle_UnitDequeued;
        }

        private void Start()
        {
            ProceedToNextTurn();
        }

        private void Update()
        {
            if (readyInput.action.triggered)
                ProceedToNextTurn();
        }
#endregion

        public void ProceedToNextTurn()
        {
            currentTurn.NextTurn();

            Castle castleToGainGold = currentTurn.faction switch
            {
                Faction.Player => castlePlayer,
                Faction.Enemy => castleEnemy,
                _ => null
            };

            Castle castleToSpawnUnit = currentTurn.faction switch
            {
                Faction.Player => castleEnemy,
                Faction.Enemy => castlePlayer,
                _ => null
            };

            if (castleToGainGold) castleToGainGold.GainGold();
            if (castleToSpawnUnit) castleToSpawnUnit.TryDequeueSpawnUnit(out _);

            battlefield.AdvanceAllUnits();
            OnTurnChanged?.Invoke(this, currentTurn);
        }

        public void SpawnPlayerUnit(UnitAsset unitAsset) => TrySpawn(Faction.Player, unitAsset);
        public void SpawnEnemyUnit(UnitAsset unitAsset) => TrySpawn(Faction.Enemy, unitAsset);

        private bool TrySpawn(Faction faction, UnitAsset unitAsset)
        {
            if (!battlefield.CanPushUnit(faction))
                return false;

            bool success = battlefield.PushUnit(faction, unitAsset);

            if (success)
                OnUnitSpawned?.Invoke(this, faction, unitAsset);

            return success;
        }

        private void Castle_UnitEnqueued(Castle castle, UnitAsset unitAsset)
        {
            Log.Info($"{castle.Faction} Castle queued up {unitAsset.name}", this);
        }

        private void Castle_UnitDequeued(Castle castle, UnitAsset unitAsset)
        {
            Faction faction = castle.Faction;
            TrySpawn(faction, unitAsset);

            Log.Info($"{faction} Castle spawned {unitAsset.name}", this);
        }
    }
}
