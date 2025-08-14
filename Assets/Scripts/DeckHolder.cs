using System.Diagnostics;
using FurkanKambay.Deckbuilding;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
            PrintDeck();
        }

        [ContextMenu("Discard Hand")]
        public void ResetToStarterDeck()
        {
            Deck.ResetToStarterDeck();
            PrintDeck();
        }

        [Conditional("UNITY_EDITOR")]
        private void PrintDeck() =>
            Debug.Log($"Deck: {Deck}");

        public bool TryUseCard(Card card)
        {
            if (!CanUseCard(card))
                return false;

            Gold -= card.CardSO.Cost;

            // TODO: other unit types
            if (card.CardSO.CardType == CardType.Unit)
                castle.SpawnUnit();

            // TODO: other card effects

            Deck.DiscardCard(card);

            if (Deck.HandPile.CardCount == 0)
                Deck.DrawHand();

            return true;
        }

        public bool CanUseCard(Card card) =>
            card.CardSO.Cost <= Gold;
    }
}
