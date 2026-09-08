using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FK.Deckowar.Core;
using FK.Deckowar.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace FK.Deckowar
{
    public class Castle : MonoBehaviour, INotifyPropertyChanged
    {
        public event Action<Castle, UnitAsset> OnUnitEnqueued;
        public event Action<Castle, UnitAsset> OnUnitDequeued;

        public event PropertyChangedEventHandler PropertyChanged;

        [Header("References")]
        [SerializeField] private TurnTimeManager turnTimeManager;

        [Header("Config")]
        [SerializeField] private Faction faction;
        [SerializeField, Min(1)] private int maxHealth = 20;
        [SerializeField, Min(0)] private float goldGainPerTurn = 1;

        public Faction Faction => faction;
        public int SpawnQueueCount => spawnQueue.Count;

        public float Gold
        {
            get => gold;
            private set => SetField(ref gold, value);
        }

        public Vitality Vitality { get; private set; }

        private readonly Queue<UnitAsset> spawnQueue = new();
        private float gold;

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

        public void EnqueueSpawnUnit(UnitAsset unit)
        {
            if (!unit)
                return;

            spawnQueue.Enqueue(unit);
            OnUnitEnqueued?.Invoke(this, unit);
        }

        public bool TryDequeueSpawnUnit(out UnitAsset unitAsset)
        {
            if (!spawnQueue.TryDequeue(out unitAsset))
                return false;

            OnUnitDequeued?.Invoke(this, unitAsset);
            return true;
        }

        private void TurnTimeManager_TurnChanged(TurnTimeManager sender)
        {
            if (sender.CurrentFaction == Faction)
                GainGold();
            else
                TryDequeueSpawnUnit(out _);
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}
