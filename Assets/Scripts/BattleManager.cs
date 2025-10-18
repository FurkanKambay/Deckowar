using System;
using Deckowar.Core;
using Deckowar.Data;
using UnityEditor;
using UnityEngine;

namespace Deckowar
{
    public sealed class BattleManager : MonoBehaviour
    {
        public event Action<BattleManager, Heading, Unit> OnUnitSpawned;

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

            var spawnedUnit = new Unit(unitSO);
            bool success = battlefield.PushUnit(heading, spawnedUnit);

            if (success)
                OnUnitSpawned?.Invoke(this, heading, spawnedUnit);

            return success;
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
