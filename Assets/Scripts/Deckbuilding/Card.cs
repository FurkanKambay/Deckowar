using System;
using UnityEngine;

namespace FK.Deckowar.Deckbuilding
{
    [Serializable]
    public class Card
    {
        [SerializeField] private CardAsset cardAsset;

        public CardAsset CardAsset => cardAsset;
        public bool IsValid => CardPile is not null;

        public Deck Deck { get; protected set; }
        public CardPile CardPile { get; protected set; }
        public int PileIndex { get; protected set; } = -1;

        internal Card(CardAsset cardAsset, Deck ownerDeck)
        {
            this.cardAsset = cardAsset;
            Deck = ownerDeck;
        }

        internal void SetPile(CardPile cardPile, int pileIndex)
        {
            CardPile = cardPile;
            PileIndex = pileIndex;
        }

        public override string ToString() =>
            $"[{PileIndex}] {cardAsset.DisplayName}";
    }
}
