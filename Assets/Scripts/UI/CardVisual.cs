using FK.Deckowar.Deckbuilding;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Color = UnityEngine.Color;

namespace FK.Deckowar.UI
{
    public sealed class CardVisual : MonoBehaviour, IPointerClickHandler
    {
        [Header("Prefab References")]
        [SerializeField] private Image background;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text costLabel;
        [SerializeField] private TMP_Text description;

        [Header("Config")]
        [SerializeField] private Sprite unitBackground;
        [SerializeField] private Sprite turretBackground;
        [SerializeField] private Color costColor;
        [SerializeField] private Color costColorInsufficient;

        [Header("State")]
        [SerializeField] private DeckHolder deckHolder;
        [SerializeField] private Card card;

        private void Update()
        {
            UpdateCostLabel();
        }

        internal void SetState(DeckHolder newDeckHolder, Card newCard)
        {
            deckHolder = newDeckHolder;
            card = newCard;

            UpdateCard();
        }

        private void UpdateCard()
        {
            if (card is null || !card.IsValid)
                return;

            background.sprite = card.CardAsset.CardType switch
            {
                CardType.Invalid => unitBackground,
                CardType.Unit => unitBackground,
                CardType.Turret => turretBackground,
                _ => unitBackground
            };

            icon.sprite = card.CardAsset.Icon;
            title.text = card.CardAsset.DisplayName;
            description.text = card.CardAsset.Description;
        }

        private void UpdateCostLabel()
        {
            if (card is null || !card.IsValid)
                return;

            int cost = card.CardAsset.Cost;
            Color color = cost <= deckHolder.Castle.Gold ? costColor : costColorInsufficient;
            string hex = ColorUtility.ToHtmlStringRGB(color);

            costLabel.text = $"<color=#{hex}>{cost}</color>";
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            bool used = deckHolder.TryUseCard(card);
        }
    }
}
