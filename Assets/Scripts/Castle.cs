using System.Collections.Generic;
using Deckowar.Data;
using UnityEditor;
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

        private void Update()
        {
            if (!vitality.IsAlive)
                return;

            if (SpawnQueueCount == 0)
                return;

            if (!MaybeSpawn())
                spawnTimer += Time.deltaTime;
        }

        [ContextMenu("Spawn Unit")]
        public void EnqueueSpawnUnit(UnitSO unit) =>
            spawnQueue.Enqueue(unit);

        private bool MaybeSpawn()
        {
            if (SpawnQueueCount == 0)
                return false;

            UnitSO queuedUnitSO = spawnQueue.Peek();

            if (spawnTimer < queuedUnitSO.SpawnDelay)
                return false;

            spawnTimer = 0;
            spawnQueue.Dequeue();

            Unit spawnedUnit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity, transform);
            spawnedUnit.Initialize(queuedUnitSO);

            return true;
        }

        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.blue;
            Handles.DrawWireDisc(transform.position + (Vector3)spawnDelta, Vector3.forward, 0.1f);
        }
    }
}
