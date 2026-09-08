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

        public void Init(DeckHolder deckHolder)
        {
            this.deckHolder = deckHolder;
        }

        private void OnEnable()
        {
            if (!deckHolder) return;

            deckHolder.Deck.OnHandDrawn += Deck_HandUpdated;
            deckHolder.Deck.OnHandDiscarded += Deck_HandUpdated;
            deckHolder.Deck.OnResetToStarterDeck += Deck_HandUpdated;
            deckHolder.Deck.OnCardDrawn += Deck_CardUpdated;
            deckHolder.Deck.OnCardDiscarded += Deck_CardUpdated;

            InitializeCards();
            UpdateUI();
        }

        private void OnDisable()
        {
            if (!deckHolder) return;

            deckHolder.Deck.OnHandDrawn -= Deck_HandUpdated;
            deckHolder.Deck.OnHandDiscarded -= Deck_HandUpdated;
            deckHolder.Deck.OnResetToStarterDeck -= Deck_HandUpdated;
            deckHolder.Deck.OnCardDrawn -= Deck_CardUpdated;
            deckHolder.Deck.OnCardDiscarded -= Deck_CardUpdated;
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

        private void Deck_HandUpdated() =>
            UpdateUI();

        private void Deck_CardUpdated(Card card) =>
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
