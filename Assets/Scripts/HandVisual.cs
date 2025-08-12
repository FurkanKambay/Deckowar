using FurkanKambay.Deckbuilding;
using UnityEngine;

namespace FurkanKambay
{
    public class HandVisual : MonoBehaviour
    {
        [Header("References - Asset")]
        [SerializeField] private CardVisual cardVisualPrefab;

        [Header("References - Scene")]
        [SerializeField] private DeckHolder deckHolder;
        [SerializeField] private Transform cardParent;

        [Header("State")]
        [SerializeField] private CardVisual[] cardVisuals;

        private void Start()
        {
            deckHolder.Deck.OnHandDrawn     += DeckHand_Updated;
            deckHolder.Deck.OnHandDiscarded += DeckHand_Updated;

            InitializeCards();
        }

        private void OnDestroy()
        {
            deckHolder.Deck.OnHandDrawn     -= DeckHand_Updated;
            deckHolder.Deck.OnHandDiscarded -= DeckHand_Updated;
        }

        private void InitializeCards()
        {
            foreach (Transform card in cardParent)
                Destroy(card.gameObject);

            cardVisuals = new CardVisual[deckHolder.Deck.HandSize];

            for (int i = 0; i < deckHolder.Deck.HandSize; i++)
            {
                CardVisual instance = Instantiate(cardVisualPrefab, cardParent);
                cardVisuals[i] = instance;

                instance.name = $"Card {i + 1}";
            }
        }

        private void DeckHand_Updated()
        {
            CardPile hand = deckHolder.Deck.HandPile;

            int missing = hand.CardCount - cardVisuals.Length;

            for (int i = 0; i < missing; i++)
            {
                CardVisual instance = Instantiate(cardVisualPrefab, cardParent);
            }

            foreach (Card card in hand.ListRO)
            {
            }
        }
    }
}
