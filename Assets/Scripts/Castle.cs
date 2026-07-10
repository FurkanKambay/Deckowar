using System;
using System.Collections.Generic;
using FK.Deckowar.Core;
using FK.Deckowar.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace FK.Deckowar
{
    public class Castle : MonoBehaviour
    {
        public event Action<Castle, UnitSO> OnUnitEnqueued;
        public event Action<Castle, UnitSO> OnUnitDequeued;

        [Header("References")]
        [SerializeField] private TurnTimeManager turnTimeManager;

        [Header("Config")]
        [SerializeField] private Faction faction;
        [SerializeField, Min(1)] private int maxHealth = 20;
        [SerializeField, Min(0)] private float goldGainPerTurn = 1;

        public Faction Faction => faction;
        public int SpawnQueueCount => spawnQueue.Count;

        public float Gold { get; private set; }
        public Vitality Vitality { get; private set; }

        private readonly Queue<UnitSO> spawnQueue = new();

        private void Awake()
        {
            Assert.IsNotNull(turnTimeManager);
            Vitality = new Vitality(maxHealth);
        }

        private void OnEnable() => turnTimeManager.OnTurnChanged += TurnTimeManager_TurnChanged;
        private void OnDisable() => turnTimeManager.OnTurnChanged -= TurnTimeManager_TurnChanged;

        public void GainGold() =>
            Gold = Mathf.Clamp(Gold + goldGainPerTurn, 0, max: 500);

        public void LoseGold(int amount) =>
            Gold = Mathf.Clamp(Gold - amount, 0, max: 500);

        public void EnqueueSpawnUnit(UnitSO unit)
        {
            if (!unit)
                return;

            spawnQueue.Enqueue(unit);
            OnUnitEnqueued?.Invoke(this, unit);
        }

        public bool TryDequeueSpawnUnit(out UnitSO unitSO)
        {
            if (!spawnQueue.TryDequeue(out unitSO))
                return false;

            OnUnitDequeued?.Invoke(this, unitSO);
            return true;
        }

        private void TurnTimeManager_TurnChanged(TurnTimeManager sender)
        {
            if (sender.CurrentFaction == Faction)
                GainGold();
            else
                TryDequeueSpawnUnit(out _);
        }
    }
}
