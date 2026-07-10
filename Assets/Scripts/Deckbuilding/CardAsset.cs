using FK.Deckowar.Data;
using UnityEngine;

namespace FK.Deckowar.Deckbuilding
{
    public enum CardType
    {
        Invalid,
        Unit,
        Turret
    }

    [CreateAssetMenu(menuName = "Card")]
    public class CardAsset : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private CardType cardType;
        [SerializeField] private string displayName;
        [SerializeField] private Sprite icon;
        [SerializeField] private string description;
        [SerializeField] private int cost;

        // only show when CardType.Unit
        [SerializeField] private UnitAsset unitAsset;

        public int Id => id;
        public CardType CardType => cardType;
        public string DisplayName => displayName;
        public Sprite Icon => icon;
        public string Description => description;
        public int Cost => cost;

        public UnitAsset UnitAsset => unitAsset;

        public Card CreateInstance(Deck ownerDeck) =>
            new(this, ownerDeck);
    }
}
