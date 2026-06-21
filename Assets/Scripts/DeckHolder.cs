using Deckowar.Deckbuilding;
using Furkan.Common;
using UnityEngine;
using UnityEngine.Assertions;

namespace Deckowar
{
    public sealed class DeckHolder : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Castle castle;

        [Header("Config")]
        [SerializeField] private DeckConfigSO deckConfigSO;

        public Castle Castle => castle;
        public Deck Deck { get; private set; }

        private void Awake()
        {
            Assert.IsNotNull(castle);
            Assert.IsNotNull(deckConfigSO);

            Deck = new Deck(deckConfigSO);

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

            castle.LoseGold(card.CardSO.Cost);

            // TODO: other unit types
            if (card.CardSO.CardType == CardType.Unit)
                castle.EnqueueSpawnUnit(card.CardSO.UnitSO);

            // TODO: other card effects

            Deck.DiscardCard(card);

            if (Deck.HandCardCount == 0)
                Deck.DrawHand();

            return true;
        }

        public bool CanUseCard(Card card) =>
            card.CardSO.Cost <= castle.Gold;

        [HideInCallstack]
        private void PrintDeck() => EditorDebug.Log($"[Deck] {castle.Faction}: {Deck:E}");
    }
}
