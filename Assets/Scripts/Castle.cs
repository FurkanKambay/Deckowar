using System.Collections.Generic;
using Deckowar.Data;
using UnityEngine;

namespace Deckowar
{
    public class Castle : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Vitality vitality;
        [SerializeField] private Unit unitPrefab;

        [Header("Config")]
        [SerializeField] private Vector2 spawnDelta;

        public Vitality Vitality => vitality;
        public int SpawnQueueCount => spawnQueue.Count;

        public float ProgressUntilNextSpawn =>
            SpawnQueueCount == 0 ? 0f : Mathf.InverseLerp(0, spawnQueue.Peek().SpawnDelay, spawnTimer);

        private Vector3 spawnPosition;
        private float spawnTimer;

        private readonly Queue<UnitSO> spawnQueue = new();

        private void Awake()
        {
            spawnPosition = transform.position + (Vector3)spawnDelta;
        }

        public void EnqueueSpawnUnit(UnitSO unit) =>
            spawnQueue.Enqueue(unit);
    }
}
