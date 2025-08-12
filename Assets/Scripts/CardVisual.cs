using FurkanKambay.Deckbuilding;
using UnityEngine;
using UnityEngine.UI;

namespace FurkanKambay
{
    public class CardVisual : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image background;
        [SerializeField] private Image icon;

        [Header("Config")]
        [SerializeField] private Color unitBackgroundColor;
        [SerializeField] private Color turretBackgroundColor;

        [Header("State")]
        [SerializeField] private Card card;

        public void SetCard(Card newCard)
        {
            card = newCard;
            UpdateCard();
        }

        [ContextMenu("Update Card")]
        public void UpdateCard()
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
    }
}
