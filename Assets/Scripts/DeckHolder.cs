using System.Diagnostics;
using FurkanKambay.Deckbuilding;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace FurkanKambay
{
    public class DeckHolder : MonoBehaviour
    {
        [SerializeField] private DeckConfigSO deckConfigSO;

        public Deck Deck { get; private set; }

        private void Awake()
        {
            Deck = new Deck(deckConfigSO);

            Deck.ResetToStarterDeck();
            PrintDeck();
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
    }
}
