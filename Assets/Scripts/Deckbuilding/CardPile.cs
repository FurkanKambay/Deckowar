using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;

namespace FurkanKambay.Deckbuilding
{
    public class CardPile
    {
        public Pile                     PileType { get; }
        public ReadOnlyCollection<Card> ListRO   { get; }

        public int  CardCount => list.Count;
        public Card LastCard  => list.Count == 0 ? null : list[^1];

        private readonly List<Card> list;

        public CardPile(Pile pileType)
        {
            PileType = pileType;
            list     = new List<Card>();
            ListRO   = list.AsReadOnly();
        }

        public void TakeAllFrom(CardPile sourcePile)
        {
            if (sourcePile is null || sourcePile == this)
                return;

            foreach (Card card in sourcePile.ListRO)
                AddCard(card, move: false);

            sourcePile.Clear();
        }

        internal bool Take(Card card) =>
            AddCard(card, move: true);

        internal void Clear() =>
            list.Clear();

        internal void Shuffle()
        {
            int targetIndex = list.Count;
            var provider    = new RNGCryptoServiceProvider();

            while (targetIndex > 1)
            {
                byte[] box = new byte[1];

                do
                    provider.GetBytes(box);
                while (!(box[0] < targetIndex * (byte.MaxValue / targetIndex)));

                int sourceIndex = box[0] % targetIndex;
                targetIndex--;

                Card sourceCard = list[sourceIndex];
                Card targetCard = list[targetIndex];

                list[sourceIndex] = targetCard;
                list[targetIndex] = sourceCard;

                // Keep pile index in sync
                sourceCard.SetPile(this, targetIndex);
                targetCard.SetPile(this, sourceIndex);
            }
        }

        private bool AddCard(Card card, bool move)
        {
            if (card is null || card.CardPile == this)
                return false;

            if (move)
                card.CardPile?.RemoveCard(card);

            card.SetPile(this, pileIndex: CardCount);
            list.Add(card);

            return true;
        }

        private void RemoveCard(Card card)
        {
            if (card is null || card.CardPile != this)
                return;

            list.RemoveAt(card.PileIndex);

            // Cascade down to adjust pile indexes
            for (int i = card.PileIndex; i < CardCount; i++)
                list[i].SetPile(this, i);
        }

        public override string ToString() =>
            $"{PileType}: {CardCount} Cards";
    }
}
