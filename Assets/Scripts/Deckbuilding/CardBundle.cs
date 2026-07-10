using System;
using UnityEngine;

namespace FK.Deckowar.Deckbuilding
{
    [Serializable]
    public record CardBundle
    {
        [SerializeField] private CardSO cardSO;
        [SerializeField, Min(0)] private int amount;

        public CardSO CardSO => cardSO;
        public int Amount => amount;
    }
}
