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

        private DeckConfigSO config;

        public Deck(DeckConfigSO config)
        {
            if (!config)
                throw new ArgumentNullException(nameof(config));

            this.config = config;

            DrawPile    = new CardPile(Pile.DrawPile);
            HandPile    = new CardPile(Pile.HandPile);
            DiscardPile = new CardPile(Pile.DiscardPile);

            ResetToStarterDeck_WithoutNotify();
        }

        public void FillUpHand()
        {
            bool hasDrawn     = false;
            int  missingCount = config.HandSize - HandPile.CardCount;

            for (int i = 0; i < missingCount; i++)
            {
                if (HandPile.CardCount >= config.HandSize)
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
            if (DrawPile.CardCount == 0)
                ReshuffleDrawPile();

            if (DrawPile.CardCount == 0)
            {
                drawnCard = null;
                return false;
            }

            Card card     = DrawPile.LastCard;
            bool hasDrawn = HandPile.Take(card);

            drawnCard = hasDrawn ? card : null;

            if (hasDrawn)
                OnCardDrawn?.Invoke(drawnCard);

            return hasDrawn;
        }

        private void ReshuffleDrawPile()
        {
            DrawPile.TakeAllFrom(DiscardPile);
            DrawPile.Shuffle();
        }

        public void DiscardHand()
        {
            DiscardPile.TakeAllFrom(HandPile);
            OnHandDiscarded?.Invoke();
        }

        private void DiscardCard(Card card)
        {
            if (card is null)
                return;

            if (card.PileIndex < 0 || card.PileIndex >= HandPile.CardCount)
                return;

            if (DiscardPile.Take(card))
                OnCardDiscarded?.Invoke(card);
        }

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
                DrawPile.Take(new Card(card.CardSO, this));

            // DrawPile.TrimExcess();

            HandPile.Clear();
            DiscardPile.Clear();
        }

        public override string ToString() =>
            $"⬆️ {DrawPile.CardCount} | 🤚 {HandPile.CardCount} | 🗑️ {DiscardPile.CardCount}";
    }
}
