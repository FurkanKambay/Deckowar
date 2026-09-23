using FK.Common;
using FK.Deckowar.Deckbuilding;
using UnityEngine;
using UnityEngine.Assertions;

namespace FK.Deckowar
{
    public sealed class DeckHolder : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private DeckConfigAsset deckConfigAsset;
        [SerializeField] private Castle castle;

        [Header("Config")]
        [SerializeField] private bool enableLogs;

        public Castle Castle => castle;
        public Deck Deck { get; private set; }

        private void Awake()
        {
            Assert.IsNotNull(deckConfigAsset);

            Deck = new Deck(deckConfigAsset);

            Deck.ResetToStarterDeck();
            Deck.DrawHand();

            PrintDeck();
        }

        public void DrawHand()
        {
            Deck.DrawHand();
            PrintDeck();
        }

        public void DiscardHand()
        {
            Deck.DiscardHand();
            // PrintDeck();
        }

        public void ResetToStarterDeck()
        {
            Deck.ResetToStarterDeck();
            PrintDeck();
        }

        public bool TryUseCard(Card card)
        {
            if (!CanUseCard(card))
                return false;

            castle.LoseGold(card.CardAsset.Cost);

            // TODO: other unit types
            if (card.CardAsset.CardType == CardType.Unit)
                castle.EnqueueSpawnUnit(card.CardAsset.UnitAsset);

            // TODO: other card effects

            Deck.DiscardCard(card);

            if (Deck.HandCardCount == 0)
                Deck.DrawHand();

            return true;
        }

        public bool CanUseCard(Card card) =>
            card.CardAsset.Cost <= castle.Gold;

        [HideInCallstack]
        private void PrintDeck()
        {
            if (enableLogs)
                Log.Info($"[Deck] {castle.Faction}: {Deck:E}", this);
        }
    }
}
