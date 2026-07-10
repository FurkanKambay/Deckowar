using FK.Deckowar.Deckbuilding;
using TMPro;
using UnityEngine;

namespace FK.Deckowar.UI
{
    public class HandVisual : MonoBehaviour
    {
        [Header("References - Asset")]
        [SerializeField] private CardVisual cardVisualPrefab;

        [Header("References - Scene")]
        [SerializeField] private DeckHolder deckHolder;
        [SerializeField] private Transform cardParent;
        [SerializeField] private TMP_Text drawPileLabel;
        [SerializeField] private TMP_Text discardPileLabel;

        [Header("State")]
        [SerializeField] private CardVisual[] cardVisuals;

        private void Start()
        {
            deckHolder.Deck.OnHandDrawn += Hand_Updated;
            deckHolder.Deck.OnHandDiscarded += Hand_Updated;
            deckHolder.Deck.OnResetToStarterDeck += Hand_Updated;
            deckHolder.Deck.OnCardDrawn += Card_Updated;
            deckHolder.Deck.OnCardDiscarded += Card_Updated;

            InitializeCards();
            UpdateUI();
        }

        private void OnDestroy()
        {
            deckHolder.Deck.OnHandDrawn -= Hand_Updated;
            deckHolder.Deck.OnHandDiscarded -= Hand_Updated;
            deckHolder.Deck.OnResetToStarterDeck -= Hand_Updated;
            deckHolder.Deck.OnCardDrawn -= Card_Updated;
            deckHolder.Deck.OnCardDiscarded -= Card_Updated;
        }

        private void InitializeCards()
        {
            foreach (Transform card in cardParent)
                Destroy(card.gameObject);

            cardVisuals = new CardVisual[deckHolder.Deck.HandSize];

            for (int i = 0; i < deckHolder.Deck.HandSize; i++)
            {
                cardVisuals[i] = Instantiate(cardVisualPrefab, cardParent);
                cardVisuals[i].name = $"Card {i + 1}";
            }
        }

        private void Hand_Updated() =>
            UpdateUI();

        private void Card_Updated(Card card) =>
            UpdateUI();

        private void UpdateUI()
        {
            Deck deck = deckHolder.Deck;
            CardPile hand = deck.HandPile;

            for (int i = 0; i < deck.HandSize; i++)
            {
                CardVisual visual = cardVisuals[i];
                visual.gameObject.SetActive(hand.HasIndex(i));

                if (hand.TryPeek(i, out Card card))
                    visual.SetState(deckHolder, card);
            }

            drawPileLabel.text = deckHolder.Deck.DrawPileCount.ToString();
            discardPileLabel.text = deckHolder.Deck.DiscardPileCount.ToString();
        }
    }
}
