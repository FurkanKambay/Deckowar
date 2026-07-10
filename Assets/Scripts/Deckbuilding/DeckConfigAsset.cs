using UnityEngine;
using Vertx.Attributes;

namespace FK.Deckowar.Deckbuilding
{
    [CreateAssetMenu(menuName = "Deck Config")]
    public sealed class DeckConfigAsset : ScriptableObject
    {
        [SerializeField, Range(1, 20)] private int handSize = 5;
        [SerializeField, Inline] private CardBundle[] starterDeck;

        public int HandSize => handSize;
        public CardBundle[] StarterDeck => starterDeck;
    }
}
