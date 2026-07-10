using System;
using UnityEngine;

namespace FK.Deckowar.Deckbuilding
{
    [Serializable]
    public struct CardBundle
    {
        [SerializeField] private CardSO cardSO;
        [SerializeField, Min(0)] private int amount;

        public CardSO CardSO => cardSO;
        public int Amount => amount;
        public bool IsValid => amount > 0 && cardSO;
    }
}
