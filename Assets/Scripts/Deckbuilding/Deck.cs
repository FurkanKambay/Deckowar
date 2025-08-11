using System;
using System.Linq;

namespace FurkanKambay.Deckbuilding
{
    [Serializable]
    public class Deck
    {
#region Events
        public event Action OnResetToStarterDeck;

        public event Action OnHandDrawn;
        public event Action OnHandDiscarded;
        public event Action OnDrawPileReshuffled;

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
            OnDrawPileReshuffled?.Invoke();
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

            foreach (DeckConfigSO.Set cardSet in config.StarterDeck)
            {
                for (int i = 0; i < cardSet.amount; i++)
                    DrawPile.Take(cardSet.cardSO.CreateInstance(this));
            }

            // DrawPile.TrimExcess();
            DrawPile.Shuffle();

            HandPile.Clear();
            DiscardPile.Clear();
        }

        public override string ToString()
        {
            int drawCount    = DrawPile.CardCount;
            int handCount    = HandPile.CardCount;
            int discardCount = DiscardPile.CardCount;

            string draws    = string.Concat(Enumerable.Repeat("⬆️",  drawCount));
            string hands    = string.Concat(Enumerable.Repeat("🤚",  handCount));
            string discards = string.Concat(Enumerable.Repeat("🗑️", discardCount));

            return $"{drawCount,2} {handCount,2} {discardCount,2} | {draws} | {hands} | {discards} |";
        }
    }
}
