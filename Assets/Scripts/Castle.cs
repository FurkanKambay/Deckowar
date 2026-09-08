using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FK.Deckowar.Core;
using FK.Deckowar.Data;
using UnityEngine;

namespace FK.Deckowar
{
    [DisallowMultipleComponent]
    public class Castle : MonoBehaviour, INotifyPropertyChanged
    {
        public event Action<Castle, UnitAsset> OnUnitEnqueued;
        public event Action<Castle, UnitAsset> OnUnitDequeued;
        public event PropertyChangedEventHandler PropertyChanged;

        [Header("Injected State")]
        [SerializeField] private CastleStatsAsset statsAsset;

        public CastleStatsAsset StatsAsset => statsAsset;
        public Faction Faction => statsAsset?.Faction ?? Faction.None;
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
            Vitality = new Vitality(statsAsset.MaxHealth);
        }

        public void GainGold() =>
            Gold = Mathf.Clamp(Gold + statsAsset.GoldGainPerTurn, 0, max: 500);

        public void LoseGold(int amount) =>
            Gold = Mathf.Clamp(Gold - amount, 0, max: 500);

        public void EnqueueSpawnUnit(UnitAsset unit)
        {
            if (!unit)
                return;

            spawnQueue.Enqueue(unit);
            OnUnitEnqueued?.Invoke(this, unit);
        }

        public bool TryDequeueSpawnUnit(out UnitAsset dequeuedUnitAsset)
        {
            if (!spawnQueue.TryDequeue(out dequeuedUnitAsset))
                return false;

            OnUnitDequeued?.Invoke(this, dequeuedUnitAsset);
            return true;
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
