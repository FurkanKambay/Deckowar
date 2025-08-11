using System;
using UnityEngine;

namespace FurkanKambay.Deckbuilding
{
    [CreateAssetMenu(menuName = "Deck Config")]
    public sealed class DeckConfigSO : ScriptableObject
    {
        [Serializable]
        public struct Set
        {
            public CardSO cardSO;

            [Min(0)]
            public int amount;
        }

        [Min(1)]
        [SerializeField] private int handSize = 5;
        [SerializeField] private Set[] starterDeck;

        public int   HandSize    => handSize;
        public Set[] StarterDeck => starterDeck;
    }
}
