using System.Collections.Generic;
using Deckowar.Deckbuilding;
using UnityEngine;

namespace Deckowar.Data
{
    [CreateAssetMenu]
    public class BotStrategySO : ScriptableObject
    {
        [SerializeField] private CardBundle[] cardBundles;

        public IReadOnlyList<CardBundle> CardBundles => cardBundles;
    }
}
