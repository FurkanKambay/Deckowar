using System;
using UnityEngine;

namespace FurkanKambay.Deckbuilding
{
    public enum Pile
    {
        Invalid,
        DrawPile,
        HandPile,
        DiscardPile
    }

    [Serializable]
    public class Card
    {
        [SerializeField] private CardSO cardSO;

        public CardSO CardSO => cardSO;

        public Deck     Deck      { get; protected set; }
        public CardPile CardPile  { get; protected set; }
        public int      PileIndex { get; protected set; } = -1;

        public Card(CardSO cardSO, Deck ownerDeck)
        {
            this.cardSO = cardSO;
            Deck        = ownerDeck;
        }

        internal void SetPile(CardPile cardPile, int pileIndex)
        {
            CardPile  = cardPile;
            PileIndex = pileIndex;
        }

        public override string ToString() =>
            $"{CardPile?.PileType}[{PileIndex}]: {cardSO.DisplayName}";
    }
}
