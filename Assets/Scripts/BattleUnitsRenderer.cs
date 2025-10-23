using Deckowar.Core;
using UnityEditor;
using UnityEngine;

namespace Deckowar
{
    public sealed class BattleUnitsRenderer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Battlefield battlefield;
        [SerializeField] private BattleManager battleManager;

        [Header("Asset References")]
        [SerializeField] private UnitAnimator playerUnitPrefab;
        [SerializeField] private UnitAnimator enemyUnitPrefab;

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
            (UnitAnimator prefab, Vector2 spawnPoint) = heading switch
            {
                Heading.West => (enemyUnitPrefab, spawnPointEnemy),
                Heading.East => (playerUnitPrefab, spawnPointPlayer),
                _ => default
            };

            UnitAnimator spawnedUnit = Instantiate(prefab, spawnPoint, Quaternion.identity, transform);
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
