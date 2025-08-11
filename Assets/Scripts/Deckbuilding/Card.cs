using System;
using UnityEngine;

namespace FurkanKambay.Deckbuilding
{
    public enum Pile
    {
        DrawPile,
        Hand,
        DiscardPile
    }

    [Serializable]
    public class Card
    {
        [SerializeField] private CardSO cardSO;

        public CardSO CardSO => cardSO;

        public Deck Deck      { get; protected set; }
        public Pile Pile      { get; protected set; }
        public int  PileIndex { get; protected set; }

        public Card(CardSO cardSO)
        {
            this.cardSO = cardSO;
        }

        public Card(Card previousCard)
        {
            cardSO = previousCard.CardSO;
        }

        internal void SetPile(Pile pile, int pileIndex)
        {
            Pile      = pile;
            PileIndex = pileIndex;
        }
    }
}
