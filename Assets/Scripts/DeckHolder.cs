using FurkanKambay.Common;
using FurkanKambay.Deckbuilding;
using UnityEngine;

namespace FurkanKambay
{
    public sealed class DeckHolder : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private DeckConfigSO deckConfigSO;
        [SerializeField] private Castle castle;

        [Header("Config")]
        [SerializeField, Min(0)] private float goldGainPerSecond;
        [SerializeField, Min(10)] private float maxGoldAmount = 200;

        public Deck Deck { get; private set; }

        public float MaxGoldAmount => maxGoldAmount;

        public float Gold
        {
            get => gold;
            private set => gold = Mathf.Clamp(value, 0, maxGoldAmount);
        }

        private float gold;

        private void Awake()
        {
            Deck = new Deck(deckConfigSO);

            Deck.ResetToStarterDeck();
            Deck.DrawHand();

            PrintDeck();
        }

        private void Update()
        {
            Gold += goldGainPerSecond * Time.deltaTime;
        }

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

            Gold -= card.CardSO.Cost;

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
            card.CardSO.Cost <= Gold;

        // ReSharper disable Unity.PerformanceAnalysis
        [HideInCallstack]
        private void PrintDeck() => EditorDebug.Log($"[Deck] {castle.Vitality.Faction}: {Deck:E}");
    }
}
