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
            UpdateCards();
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
                cardVisuals[i]      = Instantiate(cardVisualPrefab, cardParent);
                cardVisuals[i].name = $"Card {i + 1}";
            }
        }

        private void DeckHand_Updated() =>
            UpdateCards();

        private void UpdateCards()
        {
            Deck     deck = deckHolder.Deck;
            CardPile hand = deck.HandPile;

            for (int i = 0; i < deck.HandSize; i++)
            {
                CardVisual visual = cardVisuals[i];
                visual.gameObject.SetActive(hand.HasIndex(i));

                if (hand.TryPeek(i, out Card card))
                    visual.SetState(deckHolder, card);
            }
        }
    }
}
