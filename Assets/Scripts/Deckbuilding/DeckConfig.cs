using System;
using UnityEngine;

namespace FurkanKambay.Deckbuilding
{
    [Serializable]
    public class DeckConfig
    {
        [SerializeField] private int            handSize = 5;
        [SerializeField] private CardInstance[] starterDeck;

        public int            HandSize    => handSize;
        public CardInstance[] StarterDeck => starterDeck;
    }
}
