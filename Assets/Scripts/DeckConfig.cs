using System;
using FurkanKambay.Deckbuilding;
using UnityEngine;

namespace FurkanKambay
{
    [Serializable]
    public sealed class DeckConfig : DeckConfigBase
    {
        [SerializeField] private int        handSize = 5;
        [SerializeField] private CardBase[] starterDeck;

        public override int        HandSize    => handSize;
        public override CardBase[] StarterDeck => starterDeck;
    }
}
