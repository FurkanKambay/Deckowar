using System;

namespace FurkanKambay.Deckbuilding
{
    [Serializable]
    public class Deck
    {
#region Events
        public event Action OnResetToStarterDeck;

        public event Action OnHandDrawn;
        public event Action OnHandDiscarded;

        public event Action<Card> OnCardDrawn;
        public event Action<Card> OnCardDiscarded;
#endregion

        public CardPile DrawPile    { get; private set; }
        public CardPile HandPile    { get; private set; }
        public CardPile DiscardPile { get; private set; }

        public bool IsValid { get; private set; }

        private DeckConfigSO config;

        public Deck(DeckConfigSO config)
        {
            if (!config)
                throw new ArgumentNullException(nameof(config));

            this.config = config;
            IsValid     = false;

            DrawPile    = new CardPile(Pile.DrawPile);
            HandPile    = new CardPile(Pile.HandPile);
            DiscardPile = new CardPile(Pile.DiscardPile);

            ResetToStarterDeck_WithoutNotify();
        }

#region Draw Cards
        public void FillUpHand()
        {
            bool hasDrawn     = false;
            int  missingCount = config.HandSize - HandPile.Count;

            for (int i = 0; i < missingCount; i++)
            {
                if (HandPile.Count >= config.HandSize)
                    break;

                if (!TryDrawCard(out Card drawnCard))
                    continue;

                hasDrawn = true;
                OnCardDrawn?.Invoke(drawnCard);
            }

            if (hasDrawn)
                OnHandDrawn?.Invoke();
        }

        private bool TryDrawCard(out Card drawnCard)
        {
            if (DrawPile.Count == 0)
                ReshuffleDrawPile();

            if (DrawPile.Count == 0)
            {
                drawnCard = null;
                return false;
            }

            Card card     = DrawPile.Last;
            bool hasDrawn = card?.MoveTo(HandPile) ?? false;

            drawnCard = hasDrawn ? card : null;

            if (hasDrawn)
                OnCardDrawn?.Invoke(drawnCard);

            return hasDrawn;
        }

        private void ReshuffleDrawPile()
        {
            DiscardPile.DumpAllInto(DrawPile);
            DrawPile.Shuffle();
        }
#endregion

#region Discard Cards
        public void DiscardHand()
        {
            HandPile.DumpAllInto(DiscardPile);
            OnHandDiscarded?.Invoke();
        }

        private void DiscardCard(Card card)
        {
            if (card == null)
                return;

            if (card.PileIndex < 0 || card.PileIndex >= HandPile.Count)
                return;

            if (card.MoveTo(DiscardPile))
                OnCardDiscarded?.Invoke(card);
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

            // Copy cards from the starter deck
            foreach (Card card in config.StarterDeck)
            {
                var cardInstance = new Card(card.CardSO, this);
                cardInstance.MoveTo(DrawPile);
            }

            // DrawPile.TrimExcess();

            HandPile.Clear();
            DiscardPile.Clear();

            IsValid = true;
        }
#endregion

        public override string ToString() =>
            $"{DrawPile.Count} in Draw Pile | {HandPile.Count} in Hand | {DiscardPile.Count} in Discard Pile";
    }
}
