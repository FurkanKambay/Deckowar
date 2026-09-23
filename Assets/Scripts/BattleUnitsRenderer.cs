using System;
using FK.Deckowar.Core;
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

        private void OnEnable()
        {
            battleManager.OnUnitSpawned += BattleManager_UnitSpawned;
        }

        private void OnDisable()
        {
            battleManager.OnUnitSpawned -= BattleManager_UnitSpawned;
        }

        private void BattleManager_UnitSpawned(BattleManager sender, Faction faction, Unit unit)
        {
            Vector2 spawnPoint = faction switch
            {
                Faction.Player => spawnPointPlayer,
                Faction.Enemy => spawnPointEnemy,
                _ => throw new ArgumentOutOfRangeException(nameof(faction))
            };

            UnitAnimator spawnedUnit = Instantiate(unitPrefab, spawnPoint, Quaternion.identity, transform);
            spawnedUnit.Init(unit);
        }
    }
}
