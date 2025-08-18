using FurkanKambay.Data;
using UnityEngine;

namespace FurkanKambay.Deckbuilding
{
    public enum CardType
    {
        Invalid,
        Unit,
        Turret
    }

    [CreateAssetMenu(menuName = "Card")]
    public class CardSO : ScriptableObject
    {
        [SerializeField] private int      id;
        [SerializeField] private CardType cardType;
        [SerializeField] private string   displayName;
        [SerializeField] private Sprite   icon;
        [SerializeField] private string   description;
        [SerializeField] private int      cost;

        // only show when CardType.Unit
        [SerializeField] private UnitSO unitSO;

        public int      Id          => id;
        public CardType CardType    => cardType;
        public string   DisplayName => displayName;
        public Sprite   Icon        => icon;
        public string   Description => description;
        public int      Cost        => cost;

        public UnitSO UnitSO => unitSO;

        public Card CreateInstance(Deck ownerDeck) =>
            new(this, ownerDeck);
    }
}
