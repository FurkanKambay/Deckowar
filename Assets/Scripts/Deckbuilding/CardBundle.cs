using System;
using UnityEngine;

namespace FK.Deckowar.Deckbuilding
{
    [Serializable]
    public struct CardBundle
    {
        [SerializeField] private CardAsset cardAsset;
        [SerializeField, Min(0)] private int amount;

        public CardAsset CardAsset => cardAsset;
        public int Amount => amount;
        public bool IsValid => amount > 0 && cardAsset;
    }
}
