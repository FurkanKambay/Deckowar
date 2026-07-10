using System;
using System.Linq;

namespace FK.Deckowar.Deckbuilding
{
    [Serializable]
    public class Deck : IFormattable
    {
#region Events
        public event Action OnResetToStarterDeck;

        public event Action OnHandDrawn;
        public event Action OnHandDiscarded;
        public event Action OnDrawPileReshuffled;

        public event Action<Card> OnCardDrawn;
        public event Action<Card> OnCardDiscarded;
#endregion

        public CardPile DrawPile { get; private set; }
        public CardPile HandPile { get; private set; }
        public CardPile DiscardPile { get; private set; }

        public int HandSize => config.HandSize;

        public int DrawPileCount => DrawPile.Count;
        public int HandCardCount => HandPile.Count;
        public int DiscardPileCount => DiscardPile.Count;

        private DeckConfigAsset config;

        public Deck(DeckConfigAsset config)
        {
            if (!config)
                throw new ArgumentNullException(nameof(config));

            this.config = config;

            DrawPile = new CardPile(Pile.DrawPile);
            HandPile = new CardPile(Pile.HandPile);
            DiscardPile = new CardPile(Pile.DiscardPile);
        }

        public void DrawHand()
        {
            bool hasDrawn = false;
            int missingCount = config.HandSize - HandCardCount;

            for (int i = 0; i < missingCount; i++)
            {
                if (HandCardCount >= config.HandSize)
                    break;

                if (!TryDrawCard_WithoutNotify(out Card drawnCard))
                    continue;

                hasDrawn = true;
            }

            if (hasDrawn)
                OnHandDrawn?.Invoke();
        }

        public bool TryDrawCard(out Card drawnCard)
        {
            if (!TryDrawCard_WithoutNotify(out drawnCard))
                return false;

            OnCardDrawn?.Invoke(drawnCard);
            return true;
        }

        private bool TryDrawCard_WithoutNotify(out Card drawnCard)
        {
            if (DrawPileCount == 0)
                ReshuffleDrawPile();

            if (DrawPileCount == 0)
            {
                drawnCard = null;
                return false;
            }

            Card card = DrawPile.LastCard;
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

        public void DiscardCard(Card card)
        {
            if (card is null)
                return;

            if (card.PileIndex < 0 || card.PileIndex >= HandCardCount)
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

            foreach (CardBundle cardBundle in config.StarterDeck)
            {
                for (int i = 0; i < cardBundle.Amount; i++)
                    DrawPile.Take(cardBundle.CardSO.CreateInstance(this));
            }

            // DrawPile.TrimExcess();
            DrawPile.Shuffle();

            HandPile.Clear();
            DiscardPile.Clear();
        }

        public override string ToString() =>
            $"{DrawPileCount,2} {HandCardCount,2} {DiscardPileCount,2}";

        public string ToString(string format, IFormatProvider formatProvider)
        {
            format = format?.ToUpperInvariant();

            if (format is not "E")
                return ToString();

            string draws = string.Concat(Enumerable.Repeat("⬆️", DrawPileCount));
            string hands = string.Concat(Enumerable.Repeat("🤚", HandCardCount));
            string discards = string.Concat(Enumerable.Repeat("🗑️", DiscardPileCount));

            return $"{DrawPileCount,2} {HandCardCount,2} {DiscardPileCount,2} | {draws} | {hands} | {discards} |";
        }
    }
}
