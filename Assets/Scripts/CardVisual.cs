using FurkanKambay.Deckbuilding;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FurkanKambay
{
    public class CardVisual : MonoBehaviour, IPointerClickHandler
    {
        [Header("Prefab References")]
        [SerializeField] private Image background;
        [SerializeField] private Image icon;

        [Header("Config")]
        [SerializeField] private Color unitBackgroundColor;
        [SerializeField] private Color turretBackgroundColor;

        [Header("State")]
        [SerializeField] private DeckHolder deckHolder;
        [SerializeField] private Card card;

        internal void SetState(DeckHolder newDeckHolder, Card newCard)
        {
            deckHolder = newDeckHolder;
            card       = newCard;

            UpdateCard();
        }

        [ContextMenu("Update Card")]
        private void UpdateCard()
        {
            if (card is null || !card.IsValid)
                return;

            background.color = card.CardSO.CardType switch
            {
                CardType.Invalid => Color.black,
                CardType.Unit    => unitBackgroundColor,
                CardType.Turret  => turretBackgroundColor,
                _                => Color.black
            };

            icon.sprite = card.CardSO.Icon;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("OnPointerClick");
            deckHolder.TryUseCard(card);
        }
    }
}
