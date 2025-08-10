using System;
using UnityEngine;

namespace FurkanKambay.Deckbuilding
{
    public class CardInstance : ICardInstance
    {
        Deck ICardInstance.Deck      { get; set; }
        Pile ICardInstance.Pile      { get; set; }
        int ICardInstance. PileIndex { get; set; }
    }

    [Serializable]
    public class DeckConfig : IDeckConfig
    {
        [SerializeField] private int             handSize = 5;
        [SerializeField] private ICardInstance[] starterDeck;

        public int             HandSize    => handSize;
        public ICardInstance[] StarterDeck => starterDeck;
    }
}
