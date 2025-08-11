using System;
using FurkanKambay.Deckbuilding;
using UnityEngine;

namespace FurkanKambay
{
    [Serializable]
    public class Card : CardBase
    {
        [SerializeField] private CardSO cardSO;

        public CardSO CardSO => cardSO;
    }

    [CreateAssetMenu]
    public class CardSO : ScriptableObject
    {
        [SerializeField] private int    id;
        [SerializeField] private string displayName;
        [SerializeField] private Sprite icon;
        [SerializeField] private string description;
        [SerializeField] private int    cost;

        public int    Id          => id;
        public string DisplayName => displayName;
        public Sprite Icon        => icon;
        public string Description => description;
        public int    Cost        => cost;

        public static CardBase CreateInstance()
        {
            return new CardBase();
        }
    }
}
