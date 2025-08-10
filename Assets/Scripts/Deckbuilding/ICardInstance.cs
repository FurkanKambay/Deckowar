namespace FurkanKambay.Deckbuilding
{
    public enum Pile
    {
        DrawPile,
        Hand,
        DiscardPile
    }

    public interface ICardInstance
    {
        public Deck Deck      { get; protected internal set; }
        public Pile Pile      { get; protected set; }
        public int  PileIndex { get; protected set; }

        protected internal void SetPile(Pile pile, int pileIndex)
        {
            Pile      = pile;
            PileIndex = pileIndex;
        }
    }
}
