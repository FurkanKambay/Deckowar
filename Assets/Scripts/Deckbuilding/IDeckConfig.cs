namespace FurkanKambay.Deckbuilding
{
    public interface IDeckConfig
    {
        public ICardInstance[] StarterDeck { get; }
        public int             HandSize    { get; }
    }
}
