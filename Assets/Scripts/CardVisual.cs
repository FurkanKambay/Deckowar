using FurkanKambay.Deckbuilding;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FurkanKambay
{
    public class CardVisual : MonoBehaviour, IPointerClickHandler
    {
        [Header("Prefab References")]
        [SerializeField] private Image background;
        [SerializeField] private Image    icon;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text cost;
        [SerializeField] private TMP_Text description;

        [Header("Config")]
        [SerializeField] private Sprite unitBackground;
        [SerializeField] private Sprite turretBackground;

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

            background.sprite = card.CardSO.CardType switch
            {
                CardType.Invalid => unitBackground,
                CardType.Unit    => unitBackground,
                CardType.Turret  => turretBackground,
                _                => unitBackground
            };

            icon.sprite      = card.CardSO.Icon;
            title.text       = card.CardSO.DisplayName;
            description.text = card.CardSO.Description;
            cost.text        = card.CardSO.Cost.ToString();
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            bool used = deckHolder.TryUseCard(card);
        }
    }
}
