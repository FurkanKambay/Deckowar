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

            drawnCard = MoveCard(DrawPile[^1], Pile.Hand);
            OnCardDrawn?.Invoke(drawnCard);
            return true;
        }

        private void ReshuffleDrawPile()
        {
            DumpAll(Pile.DiscardPile, Pile.DrawPile);
            DrawPile.Shuffle();
        }
#endregion

#region Discard Cards
        public void DiscardHand()
        {
            DumpAll(Pile.Hand, Pile.DiscardPile);
            OnHandDiscarded?.Invoke();
        }

        private void DiscardCard(ICardInstance card)
        {
            if (card == null)
                return;

            if (card.PileIndex < 0 || card.PileIndex >= Hand.Count)
                return;

            ICardInstance discardedCard = MoveCard(Hand[^1], Pile.DiscardPile);
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
            if (!TryGetList(pile, out List<ICardInstance> list))
                return;

            list.Shuffle();

            // Fix PileIndex values
            for (int i = 0; i < list.Count; i++)
            {
                ICardInstance card = list[i];
                card.SetPile(pile, i);
            }
        }

        protected void DumpAll(Pile sourcePile, Pile targetPile)
        {
            if (!TryGetList(sourcePile, out List<ICardInstance> sourceList))
                return;

            if (!TryGetList(targetPile, out List<ICardInstance> targetList))
                return;

            foreach (ICardInstance card in sourceList)
            {
                // TODO: Retained Cards
                targetList.Add(card);
                card.SetPile(targetPile, targetList.Count - 1);
            }

            sourceList.Clear();
        }

        protected ICardInstance MoveCard(ICardInstance card, Pile targetPile)
        {
            if (TryGetList(card.Pile, out List<ICardInstance> sourceList))
                return null;

            if (TryGetList(targetPile, out List<ICardInstance> targetList))
                return null;

            sourceList.RemoveAt(card.PileIndex);
            targetList.Add(card);

            card.SetPile(targetPile, targetList.Count - 1);
            return card;
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
#endregion

        public override string ToString() =>
            $"{DrawPile.Count} in Draw Pile | {Hand.Count} in Hand | {DiscardPile.Count} in Discard Pile";
    }
}
