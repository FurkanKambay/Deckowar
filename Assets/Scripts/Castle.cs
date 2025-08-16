using UnityEditor;
using UnityEngine;

namespace FurkanKambay
{
    public class Castle : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Unit unitPrefab;

        [Header("Config")]
        [SerializeField] private Vector2 spawnDelta;
        [SerializeField] private float spawnDelay = 1f;

        private Vector3 spawnPosition;
        private float   spawnTimer;
        private int     spawnQueueCount;

        private void Awake()
        {
            spawnPosition = transform.position + (Vector3)spawnDelta;
        }

        private void Update()
        {
            spawnTimer += Time.deltaTime;
            MaybeSpawn();
        }

        [ContextMenu("Spawn Unit")]
        public void EnqueueSpawnUnit() =>
            spawnQueueCount++;

        private void MaybeSpawn()
        {
            if (spawnQueueCount == 0 || spawnTimer < spawnDelay)
                return;

            spawnTimer = 0;
            spawnQueueCount--;

            Unit unit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity, transform);
        }

        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.blue;
            Handles.DrawWireDisc(spawnPosition, Vector3.forward, 0.1f);
        }
    }
}
