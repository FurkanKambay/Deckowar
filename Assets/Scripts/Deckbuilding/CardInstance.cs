using System;

namespace FurkanKambay.Deckbuilding
{
    public enum Pile
    {
        DrawPile,
        Hand,
        DiscardPile
    }

    [Serializable]
    public class CardInstance
    {
        public Pile Pile      { get; protected set; }
        public int  PileIndex { get; protected set; }

        internal void SetPile(Pile pile, int pileIndex)
        {
            Pile      = pile;
            PileIndex = pileIndex;
        }
    }
}
