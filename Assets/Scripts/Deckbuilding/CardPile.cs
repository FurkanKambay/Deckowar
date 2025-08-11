using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;

namespace FurkanKambay.Deckbuilding
{
    public class CardPile
    {
        public Pile                     PileType { get; }
        public ReadOnlyCollection<Card> ListRO   { get; }

        public int  Count => list.Count;
        public Card Last  => list[^1];

        private readonly List<Card> list;

        public CardPile(Pile pileType)
        {
            PileType = pileType;
            list     = new List<Card>();
            ListRO   = list.AsReadOnly();
        }

        public void DumpAllInto(CardPile targetPile)
        {
            foreach (Card card in list)
                targetPile.Add(card);

            list.Clear();
        }

        internal bool Add(Card card)
        {
            if (card.CardPile == this)
                return false;

            card.SetPile(this, Count);
            list.Add(card);

            return true;
        }

        internal bool Remove(Card card)
        {
            if (PileType != card.CardPile?.PileType)
                return false;

            list.RemoveAt(card.PileIndex);
            card.SetPile(null, -1);

            return true;
        }

        internal void Clear()
        {
            list.Clear();
        }

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
    }
}
