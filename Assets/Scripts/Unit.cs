using System;
using System.Diagnostics.CodeAnalysis;
using FK.Deckowar.Core;
using FK.Deckowar.Data;
using UnityEngine;

namespace FK.Deckowar
{
    public sealed class Unit : MonoBehaviour
    {
        [SerializeField] private UnitAsset unitAsset;
        [SerializeField] private Faction faction;

        public UnitAsset UnitAsset => unitAsset;
        public Faction Faction => faction;
        public Vitality Vitality { get; private set; }

        private Vector3 destination;

        internal void Init(UnitAsset unitAsset, Castle castle)
        {
            this.unitAsset = unitAsset ? unitAsset : throw new ArgumentNullException(nameof(unitAsset));
            this.faction = castle?.Faction ?? throw new ArgumentNullException(nameof(castle));
            Vitality = new Vitality(unitAsset.MaxHealth);

            name = $"{faction}[{unitAsset.name}]";

            destination = castle.transform.position;
            transform.position = destination;
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * 10f);
        }

        public void MoveTo(Vector2 targetPosition)
        {
            destination = targetPosition;
        }

        public static implicit operator bool([MaybeNullWhen(false), NotNullWhen(true)] Unit self)
        {
            return self != null && self.UnitAsset && self.Faction is not Faction.None;
        }
    }
}
