using Deckowar.Core;
using Deckowar.Data;
using UnityEditor;
using UnityEngine;

namespace Deckowar
{
    public sealed class BattleManager : MonoBehaviour
    {
        [Header("Asset References")]
        [SerializeField] private Unit unitPrefabPlayer;
        [SerializeField] private Unit unitPrefabEnemy;

        [Header("References")]
        [SerializeField] private Battlefield battlefield;

        private Vector2 spawnPointPlayer;
        private Vector2 spawnPointEnemy;

        private void Awake()
        {
            LocateSpawnPoints();
        }

        public void SpawnEastward(UnitSO unitSO) => TrySpawn(Heading.East, unitSO);
        public void SpawnWestward(UnitSO unitSO) => TrySpawn(Heading.West, unitSO);

        private bool TrySpawn(Heading heading, UnitSO unitSO)
        {
            if (!battlefield.CanPushUnit(heading))
                return false;

            (Unit prefab, Vector3 spawnPosition) = heading switch
            {
                Heading.West => (unitPrefabEnemy, spawnPointEnemy),
                Heading.East => (unitPrefabPlayer, spawnPointPlayer),
                _ => default
            };

            var spawnedUnit = new Unit(unitSO);

            return battlefield.PushUnit(heading, spawnedUnit);
        }

        private void LocateSpawnPoints()
        {
            int x = battlefield.CellCount - 1; // ! coupled to the visuals
            spawnPointPlayer = transform.TransformPoint(-x, 0, 0);
            spawnPointEnemy = transform.TransformPoint(+x, 0, 0);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            LocateSpawnPoints();

            Handles.color = Color.limeGreen;
            Handles.DrawWireDisc(spawnPointPlayer, Vector3.forward, 0.1f);

            Handles.color = Color.softRed;
            Handles.DrawWireDisc(spawnPointEnemy, Vector3.forward, 0.1f);
        }
#endif
    }
}
