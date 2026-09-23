using System;
using FK.Common;
using FK.Common.Extensions;
using FK.Deckowar.Core;
using FK.Deckowar.Data;
using UnityEditor;
using UnityEngine;

namespace FK.Deckowar
{
    public sealed class Battlefield : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BattleTile battleTilePrefab;
        [SerializeField] private Unit unitPrefab;
        [SerializeField] private Transform unitsParent;

        [Header("Config")]
        [SerializeField, Range(2, 10)] private int rankCount = 5;

        public int RankCount => rankCount;

        private BattleTile[] tiles;

        private void Awake()
        {
            tiles = new BattleTile[rankCount];
            InitializeTiles();
        }

        private void InitializeTiles()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform tileTransform = transform.GetChild(i);
                if (!tileTransform) continue;

                if (i >= rankCount)
                    Destroy(tileTransform.gameObject);
                else
                {
                    BattleTile tile = tileTransform.GetComponent<BattleTile>();
                    InitializeTile(tile, rank: i);
                }
            }

            Array.Resize(ref tiles, rankCount);

            for (int rank = 0; rank < tiles.Length; rank++)
            {
                BattleTile tile = tiles[rank];
                if (!tile) tile = Instantiate(battleTilePrefab, transform);
                InitializeTile(tile, rank);
            }
        }

        private void InitializeTile(BattleTile tile, int rank)
        {
            tiles[rank] = tile;
            tile.name = $"Tile {rank}";
            tile.transform.localPosition = GetTilePosition(rank);
            tile.Init(rank);
        }

#region Advancing Units
        public void AdvanceAllUnits()
        {
            Log.Info("Advancing all units", this);
            AdvanceUnits(Faction.Player);
            AdvanceUnits(Faction.Enemy);
        }

        private void AdvanceUnits(Faction faction)
        {
            for (int rank = tiles.Length - 1; rank >= 0; rank--)
                AdvanceUnit(faction, rank);
        }

        private void AdvanceUnit(Faction faction, int rank)
        {
            if (rank < 0 || rank >= tiles.Length)
                return;

            Unit unit = GetUnitAtRank(faction, rank);
            if (!unit) return;

            // only move our own faction's units
            if (unit.Faction != faction)
                return;

            Unit blockingUnit = GetUnitInFront(faction, rank);
            if (blockingUnit) return;

            bool moveSuccess = SetUnitAtRank(faction, rank + 1, unit);
            if (!moveSuccess) return;

            bool success = RemoveUnitAtRank(faction, rank);
        }
#endregion

        public bool CanPushUnit(Faction faction)
        {
            Unit unit = GetUnitAtRank(faction, 0);
            return !unit;
        }

        public bool PushUnit(Castle castle, UnitAsset unitAsset)
        {
            if (!castle || castle.Faction is Faction.None || !CanPushUnit(castle.Faction))
                return false;

            Unit spawnedUnit = Instantiate(unitPrefab, unitsParent);
            spawnedUnit.Init(unitAsset, castle);

            return SetUnitAtRank(castle.Faction, 0, spawnedUnit);
        }

        private Unit GetUnitInFront(Faction faction, int rank) =>
            GetUnitAtRank(faction, rank + 1);

        private Unit GetUnitAtRank(Faction faction, int rank)
        {
            if (faction is Faction.None || rank < 0 || rank >= tiles.Length)
                return null;

            Index index = faction is Faction.Player ? rank : ^(rank + 1);
            return tiles[index].Unit;
        }

        private bool RemoveUnitAtRank(Faction faction, int rank) =>
            SetUnitAtRank(faction, rank, null);

        private bool SetUnitAtRank(Faction faction, int rank, Unit unit)
        {
            if (faction is Faction.None || rank < 0 || rank >= tiles.Length)
                return false;

            Index index = faction is Faction.Player ? rank : ^(rank + 1);
            tiles[index].SetUnit(unit);
            return true;
        }

        private Vector3 GetTilePosition(int rank) => transform.TransformPoint((rank * 2) - rankCount + 1, 0f, 0f);

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
                InitializeTiles();
        }

        private void OnDrawGizmos()
        {
            // Battlefield bounds
            var battlefieldSize = new Vector2(rankCount * 2, 2);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, battlefieldSize);

            // Tile ranks
            GUIStyle labelStyle = GUI.skin.label;
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontSize = 20;

            for (int rank = 0; rank < rankCount; rank++)
            {
                Vector3 center = GetTilePosition(rank);
                Vector3 above = center + (Vector3.up * 1.5f);
                Vector3 below = center + (Vector3.down * 1f);

                Handles.DrawWireCube(center, Vector3.one * 2);
                Handles.Label(below, rank.ToString(), labelStyle);

                if (tiles[rank].Unit.Is(out Unit unit))
                    Handles.Label(above, unit.UnitAsset.name, labelStyle);
            }

            // Spawn points
            Vector3 spawnPointPlayer = GetTilePosition(0);
            Vector3 spawnPointEnemy = GetTilePosition(rankCount - 1);
            Handles.color = Color.limeGreen;
            Handles.DrawWireDisc(spawnPointPlayer, Vector3.forward, 0.35f);
            Handles.color = Color.softRed;
            Handles.DrawWireDisc(spawnPointEnemy, Vector3.forward, 0.35f);
        }
#endif
    }
}
