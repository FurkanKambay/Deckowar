using UnityEditor;
using UnityEngine;

namespace FurkanKambay
{
    public class Castle : MonoBehaviour
    {
        [SerializeField] private Unit    unitPrefab;
        [SerializeField] private Vector2 spawnDelta;

        private Vector3 spawnPosition;

        private void Awake()
        {
            spawnPosition = transform.position + (Vector3)spawnDelta;
        }

        [ContextMenu("Spawn Unit")]
        public void SpawnUnit()
        {
            Unit unit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity, transform);
        }

        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.blue;
            Handles.DrawWireDisc(spawnPosition, Vector3.forward, 0.1f);
        }
    }
}
