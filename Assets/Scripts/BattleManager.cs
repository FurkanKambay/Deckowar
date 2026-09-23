using System;
using FK.Deckowar.Core;
using FK.Deckowar.Data;
using UnityEngine;
using UnityEngine.Assertions;
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

        private void ProceedToNextTurn()
        {
            battlefield.AdvanceAllUnits();

            EndCurrentTurn();
            currentTurn.NextTurn();
            BeginCurrentTurn();

            OnTurnChanged?.Invoke(this, currentTurn);
        }

        /// <summary>
        /// Actualize the current turn's queued action(s).
        /// </summary>
        private void EndCurrentTurn()
        {
            Castle castle = GetCurrentCastle();
            if (!castle) return;

            if (!battlefield.CanPushUnit(castle.Faction))
                return;

            if (castle.TryDequeueSpawnUnit(out UnitAsset unitAsset))
                TrySpawn(castle, unitAsset);
        }

        /// <summary>
        /// Prepare the new turn before the castle can take any action.
        /// </summary>
        private void BeginCurrentTurn()
        {
            Castle castle = GetCurrentCastle();
            if (!castle) return;

            if (castle)
                castle.GainGold();
        }

        private Castle GetCurrentCastle() => currentTurn.faction switch
        {
            Faction.Player => castlePlayer,
            Faction.Enemy => castleEnemy,
            _ => null
        };

        private bool TrySpawn(Castle castle, UnitAsset unitAsset)
        {
            if (!battlefield.CanPushUnit(castle.Faction))
                return false;

            bool success = battlefield.PushUnit(castle, unitAsset);

            if (success)
                OnUnitSpawned?.Invoke(this, castle.Faction, unitAsset);

            return success;
        }
    }
}
