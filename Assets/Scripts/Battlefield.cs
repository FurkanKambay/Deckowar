using System;
using FK.Common;
using FK.Deckowar.Core;
using UnityEditor;
using UnityEngine;

namespace FK.Deckowar
{
    public sealed class Battlefield : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BattleTile battleTilePrefab;

        [Header("Config")]
        [SerializeField, Range(2, 10)] private int rankCount = 5;

        public int RankCount => rankCount;

        private BattleTile[] tiles;

        private void Awake()
        {
            tiles = new BattleTile[rankCount];
            InitializeTiles();
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
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
            for (int rank = tiles.Length; rank >= 0; rank--)
                AdvanceUnit(faction, rank);
        }

        private bool AdvanceUnit(Faction faction, int rank)
        {
            if (rank < 0 || rank >= tiles.Length)
                return false;

            Unit unit = GetUnitAtRank(faction, rank);
            if (!unit) return false;

            Unit blockingUnit = GetUnitInFront(faction, rank);
            if (blockingUnit)
                return false;

            bool moveSuccess = SetUnitAtRank(faction, rank + 1, unit);
            // if (!moveSuccess) return false;

            bool success = RemoveUnitAtRank(faction, rank);
            return success;
        }
#endregion

        public bool CanPushUnit(Faction faction)
        {
            Unit unit = GetUnitAtRank(faction, 0);
            return !unit;
        }

        public bool PushUnit(Faction faction, Unit unit)
        {
            if (!CanPushUnit(faction))
                return false;
            return SetUnitAtRank(faction, 0, unit);
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

        private Vector3 GetTilePosition(int rank) => new Vector2((rank * 2) - rankCount + 1, 0);

        private void OnDrawGizmos()
        {
            var size = new Vector2(rankCount * 2, 2);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, size);

            for (int rank = 0; rank < rankCount; rank++)
            {
                Vector3 center = GetTilePosition(rank);
                Handles.DrawWireCube(center, Vector3.one * 2);
                Handles.Label(center, rank.ToString());
            }
        }
    }
}
