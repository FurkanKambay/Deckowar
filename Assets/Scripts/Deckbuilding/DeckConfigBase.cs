namespace FurkanKambay.Deckbuilding
{
    public abstract class DeckConfigBase
    {
        public abstract int        HandSize    { get; }
        public abstract CardBase[] StarterDeck { get; }
    }
}
