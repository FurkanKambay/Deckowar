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

        public event Action<CardBase> OnCardDrawn;
        public event Action<CardBase> OnCardDiscarded;
#endregion

        public List<CardBase> DrawPile    { get; private set; }
        public List<CardBase> Hand        { get; private set; }
        public List<CardBase> DiscardPile { get; private set; }

        public bool IsValid { get; private set; }

        private DeckConfigBase config;

        public Deck(DeckConfigBase config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));

            IsValid     = false;
            DrawPile    = new List<CardBase>();
            Hand        = new List<CardBase>();
            DiscardPile = new List<CardBase>();

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

                if (!TryDrawCard(out CardBase drawnCard))
                    continue;

                hasDrawn = true;
                OnCardDrawn?.Invoke(drawnCard);
            }

            if (hasDrawn)
                OnHandDrawn?.Invoke();
        }

        private bool TryDrawCard(out CardBase drawnCard)
        {
            if (DrawPile.Count == 0)
                ReshuffleDrawPile();

            if (DrawPile.Count == 0)
            {
                drawnCard = null;
                return false;
            }

            drawnCard = MoveCard(DrawPile[^1], Pile.Hand);
            OnCardDrawn?.Invoke(drawnCard);
            return true;
        }

        private void ReshuffleDrawPile()
        {
            MoveAll(Pile.DiscardPile, Pile.DrawPile);
            DrawPile.Shuffle();
        }
#endregion

#region Discard Cards
        public void DiscardHand()
        {
            MoveAll(Pile.Hand, Pile.DiscardPile);
            OnHandDiscarded?.Invoke();
        }

        private void DiscardCard(CardBase card)
        {
            if (card == null)
                return;

            if (card.PileIndex < 0 || card.PileIndex >= Hand.Count)
                return;

            CardBase discardedCard = MoveCard(Hand[^1], Pile.DiscardPile);
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

#region List Operations
        protected void Shuffle(Pile pile)
        {
            if (!TryGetList(pile, out List<CardBase> list))
                return;

            list.Shuffle();

            // Fix PileIndex values
            for (int i = 0; i < list.Count; i++)
            {
                CardBase card = list[i];
                card.SetPile(pile, i);
            }
        }

        protected CardBase MoveCard(CardBase card, Pile targetPile)
        {
            if (TryGetList(card.Pile, out List<CardBase> sourceList))
                return null;

            if (TryGetList(targetPile, out List<CardBase> targetList))
                return null;

            sourceList.RemoveAt(card.PileIndex);
            targetList.Add(card);

            card.SetPile(targetPile, targetList.Count - 1);
            return card;
        }

        protected void MoveAll(Pile sourcePile, Pile targetPile)
        {
            if (!TryGetList(sourcePile, out List<CardBase> sourceList))
                return;

            if (!TryGetList(targetPile, out List<CardBase> targetList))
                return;

            foreach (CardBase card in sourceList)
            {
                // TODO: Retained Cards
                targetList.Add(card);
                card.SetPile(targetPile, targetList.Count - 1);
            }

            sourceList.Clear();
        }

        protected bool TryGetList(Pile pile, out List<CardBase> list)
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
#endregion

        public override string ToString() =>
            $"{DrawPile.Count} in Draw Pile | {Hand.Count} in Hand | {DiscardPile.Count} in Discard Pile";
    }
}
