using System.Collections.Generic;
using FurkanKambay.Deckbuilding;
using UnityEngine;

namespace FurkanKambay.Data
{
    [CreateAssetMenu]
    public class BotStrategySO : ScriptableObject
    {
        [SerializeField] private CardBundle[] cardBundles;

        public IReadOnlyList<CardBundle> CardBundles => cardBundles;
    }
}
