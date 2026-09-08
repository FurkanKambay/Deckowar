using System;
using FK.Deckowar.Core;
using UnityEditor;
using UnityEngine;

namespace FK.Deckowar
{
    public sealed class BattleUnitsRenderer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Battlefield battlefield;
        [SerializeField] private BattleManager battleManager;

        [Header("Asset References")]
        [SerializeField] private UnitAnimator unitPrefab;

        private Vector2 spawnPointPlayer;
        private Vector2 spawnPointEnemy;

        private void Awake()
        {
            LocateSpawnPoints();
        }

        private void OnEnable()
        {
            battleManager.OnUnitSpawned += BattleManager_UnitSpawned;
        }

        private void OnDisable()
        {
            battleManager.OnUnitSpawned -= BattleManager_UnitSpawned;
        }

        private void BattleManager_UnitSpawned(BattleManager sender, Heading heading, Unit unit)
        {
            Vector2 spawnPoint = heading switch
            {
                Heading.West => spawnPointEnemy,
                Heading.East => spawnPointPlayer,
                _ => throw new ArgumentOutOfRangeException(nameof(heading))
            };

            UnitAnimator spawnedUnit = Instantiate(unitPrefab, spawnPoint, Quaternion.identity, transform);
            spawnedUnit.Init(unit);
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
