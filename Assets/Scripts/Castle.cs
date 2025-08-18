using System.Collections.Generic;
using FurkanKambay.Data;
using UnityEditor;
using UnityEngine;

namespace FurkanKambay
{
    public class Castle : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Vitality vitality;
        [SerializeField] private Unit unitPrefab;

        [Header("Config")]
        [SerializeField] private Vector2 spawnDelta;
        [SerializeField] private float spawnDelay = 1f;

        public int SpawnQueueCount => spawnQueue.Count;

        private Vector3 spawnPosition;
        private float   spawnTimer;

        private readonly Queue<UnitSO> spawnQueue = new();

        private void Awake()
        {
            spawnPosition = transform.position + (Vector3)spawnDelta;
        }

        private void Update()
        {
            if (!vitality.IsAlive)
                return;

            spawnTimer += Time.deltaTime;
            MaybeSpawn();
        }

        [ContextMenu("Spawn Unit")]
        public void EnqueueSpawnUnit(UnitSO unit) =>
            spawnQueue.Enqueue(unit);

        private void MaybeSpawn()
        {
            if (SpawnQueueCount == 0 || spawnTimer < spawnDelay)
                return;

            spawnTimer = 0;
            UnitSO queuedUnitSO = spawnQueue.Dequeue();

            Unit spawnedUnit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity, transform);
            spawnedUnit.SetData(queuedUnitSO);
        }

        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.blue;
            Handles.DrawWireDisc(spawnPosition, Vector3.forward, 0.1f);
        }
    }
}
