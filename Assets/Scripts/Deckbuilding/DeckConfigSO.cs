using UnityEngine;

namespace Deckowar.Deckbuilding
{
    [CreateAssetMenu(menuName = "Deck Config")]
    public sealed class DeckConfigSO : ScriptableObject
    {
        [Min(1)]
        [SerializeField] private int handSize = 5;
        [SerializeField] private CardBundle[] starterDeck;

        public int HandSize => handSize;
        public CardBundle[] StarterDeck => starterDeck;
    }
}
