using System.Collections.Generic;
using Deckowar.Core;
using Deckowar.Data;
using UnityEngine;

namespace Deckowar
{
    public class Castle : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private Faction faction;
        [SerializeField, Min(1)] private int maxHealth = 20;

        public Faction Faction => faction;
        public Vitality Vitality { get; private set; }

        public int SpawnQueueCount => spawnQueue.Count;

        public float ProgressUntilNextSpawn =>
            SpawnQueueCount == 0 ? 0f : Mathf.InverseLerp(0, spawnQueue.Peek().SpawnDelay, spawnTimer);

        private float spawnTimer;

        private readonly Queue<UnitSO> spawnQueue = new();

        private void Awake()
        {
            Vitality = new Vitality(maxHealth);
        }

        public void EnqueueSpawnUnit(UnitSO unit) =>
            spawnQueue.Enqueue(unit);
    }
}
