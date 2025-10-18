using System.Collections.Generic;
using Deckowar.Data;
using UnityEngine;

namespace Deckowar
{
    public class Castle : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Vitality vitality;

        [Header("Config")]
        [SerializeField] private Vector2 spawnDelta;

        public Vitality Vitality => vitality;
        public int SpawnQueueCount => spawnQueue.Count;

        public float ProgressUntilNextSpawn =>
            SpawnQueueCount == 0 ? 0f : Mathf.InverseLerp(0, spawnQueue.Peek().SpawnDelay, spawnTimer);

        private float spawnTimer;

        private readonly Queue<UnitSO> spawnQueue = new();

        public void EnqueueSpawnUnit(UnitSO unit) =>
            spawnQueue.Enqueue(unit);
    }
}
