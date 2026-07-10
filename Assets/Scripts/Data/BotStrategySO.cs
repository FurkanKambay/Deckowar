using System.Collections.Generic;
using FK.Deckowar.Deckbuilding;
using UnityEngine;

namespace FK.Deckowar.Data
{
    [CreateAssetMenu]
    public class BotStrategySO : ScriptableObject
    {
        [SerializeField] private CardBundle[] cardBundles;

        public IReadOnlyList<CardBundle> CardBundles => cardBundles;
    }
}
