using Deckowar.Common;
using Deckowar.Deckbuilding;
using UnityEngine;
using UnityEngine.Assertions;

namespace Deckowar
{
    public sealed class DeckHolder : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TurnTimeManager turnTimeManager;
        [SerializeField] private Castle castle;

        [Header("Config")]
        [SerializeField] private DeckConfigSO deckConfigSO;

        public Castle Castle => castle;
        public Deck Deck { get; private set; }

        private void Awake()
        {
            Assert.IsNotNull(turnTimeManager);
            Assert.IsNotNull(castle);
            Assert.IsNotNull(deckConfigSO);

            Deck = new Deck(deckConfigSO);

            Deck.ResetToStarterDeck();
            Deck.DrawHand();

            PrintDeck();
        }

        private void OnEnable() => turnTimeManager.OnTurnChanged += TurnTimeManager_TurnChanged;
        private void OnDisable() => turnTimeManager.OnTurnChanged -= TurnTimeManager_TurnChanged;

        [ContextMenu("Draw Hand")]
        public void DrawHand()
        {
            Deck.DrawHand();
            PrintDeck();
        }

        [ContextMenu("Discard Hand")]
        public void DiscardHand()
        {
            Deck.DiscardHand();
            // PrintDeck();
        }

        [ContextMenu("Discard Hand")]
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

        private void TurnTimeManager_TurnChanged(TurnTimeManager sender)
        {
            if (sender.CurrentFaction == castle.Faction)
                castle.GainGold();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        [HideInCallstack]
        private void PrintDeck() => EditorDebug.Log($"[Deck] {castle.Faction}: {Deck:E}");
    }
}
