using System.Collections.Generic;
using FK.Deckowar.Deckbuilding;
using UnityEngine;

namespace FK.Deckowar.Data
{
    [CreateAssetMenu]
    public class BotStrategyAsset : ScriptableObject
    {
        [SerializeField] private CardBundle[] cardBundles;

        public IReadOnlyList<CardBundle> CardBundles => cardBundles;
    }
}
