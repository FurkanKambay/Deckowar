using UnityEngine;

namespace FurkanKambay.Deckbuilding
{
    [CreateAssetMenu(menuName = "Deck Config")]
    public sealed class DeckConfigSO : ScriptableObject
    {
        [Min(1)]
        [SerializeField] private int handSize = 5;
        [SerializeField] private Card[] starterDeck;

        public int    HandSize    => handSize;
        public Card[] StarterDeck => starterDeck;
    }
}
