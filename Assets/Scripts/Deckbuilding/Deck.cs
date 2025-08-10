using System;
using System.Collections.Generic;
using FurkanKambay.Extensions;

namespace FurkanKambay.Deckbuilding
{
    [Serializable]
    public class Deck
    {
#region Events
        public event Action OnResetToStarterDeck;

        public event Action OnHandDrawn;
        public event Action OnHandDiscarded;

        public event Action<ICardInstance> OnCardDrawn;
        public event Action<ICardInstance> OnCardDiscarded;
#endregion

        public List<ICardInstance> DrawPile    { get; private set; }
        public List<ICardInstance> Hand        { get; private set; }
        public List<ICardInstance> DiscardPile { get; private set; }

        public bool IsValid { get; private set; }

        private IDeckConfig config;

        public Deck(IDeckConfig config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));

            IsValid     = false;
            DrawPile    = new List<ICardInstance>();
            Hand        = new List<ICardInstance>();
            DiscardPile = new List<ICardInstance>();

            ResetToStarterDeck_WithoutNotify();
        }

#region Draw Cards
        public void FillUpHand()
        {
            bool hasDrawn     = false;
            int  missingCount = config.HandSize - Hand.Count;

            for (int i = 0; i < missingCount; i++)
            {
                if (Hand.Count >= config.HandSize)
                    break;

                if (!TryDrawCard(out ICardInstance drawnCard))
                    continue;

                hasDrawn = true;
                OnCardDrawn?.Invoke(drawnCard);
            }

            if (hasDrawn)
                OnHandDrawn?.Invoke();
        }

        private bool TryDrawCard(out ICardInstance drawnCard)
        {
            if (DrawPile.Count == 0)
                ReshuffleDrawPile();

            if (DrawPile.Count == 0)
            {
                drawnCard = null;
                return false;
            }

            drawnCard = Hand.TakeItemFrom(DrawPile, DrawPile.Count - 1);
            drawnCard.SetPile(Pile.Hand, Hand.Count - 1);

            OnCardDrawn?.Invoke(drawnCard);
            return true;
        }

        private void ReshuffleDrawPile()
        {
            DiscardPile.DumpInto(DrawPile);
            DrawPile.Shuffle();
        }
#endregion

#region Discard Cards
        public void DiscardHand()
        {
            // TODO: Retained cards
            Hand.DumpInto(DiscardPile);
            OnHandDiscarded?.Invoke();
        }

        private void DiscardCard(ICardInstance card)
        {
            if (card == null)
                return;

            if (card.PileIndex < 0 || card.PileIndex >= Hand.Count)
                return;

            ICardInstance discardedCard = DiscardPile[card.PileIndex];
            Hand.Remove(discardedCard);

            // ICardInstance discardedCard = DiscardPile.TakeItemFrom(Hand, card.PileIndex);
            discardedCard.SetPile(Pile.DiscardPile, DiscardPile.Count - 1);

            OnCardDiscarded?.Invoke(discardedCard);
        }
#endregion

#region Reset Deck
        public void ResetToStarterDeck()
        {
            ResetToStarterDeck_WithoutNotify();
            OnResetToStarterDeck?.Invoke();
        }

        private void ResetToStarterDeck_WithoutNotify()
        {
            DrawPile.Clear();
            DrawPile.AddRange(config.StarterDeck);
            DrawPile.TrimExcess();

            Hand.Clear();
            DiscardPile.Clear();

            IsValid = true;
        }
#endregion

        protected void AddCardToPile(ICardInstance card, Pile pile)
        {
            if (TryGetList(pile, out List<ICardInstance> list))
                return;

            card.SetPile(pile, pileIndex: list.Count);
            list.Add(card);
        }

        protected bool TryGetList(Pile pile, out List<ICardInstance> list)
        {
            list = pile switch
            {
                Pile.DrawPile    => DrawPile,
                Pile.Hand        => Hand,
                Pile.DiscardPile => DiscardPile,
                _                => null
            };

            return list != null;
        }

        public override string ToString()
        {
            return $"{DrawPile.Count} in Draw Pile | {Hand.Count} in Hand | {DiscardPile.Count} in Discard Pile";
        }
    }
}
