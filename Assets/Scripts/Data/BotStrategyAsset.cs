using System.Collections.Generic;
using FK.Deckowar.Deckbuilding;
using UnityEngine;
using Vertx.Attributes;

namespace FK.Deckowar.Data
{
    [CreateAssetMenu]
    public class BotStrategyAsset : ScriptableObject
    {
        [SerializeField, Inline] private CardBundle[] cardBundles;

        public IReadOnlyList<CardBundle> CardBundles => cardBundles;
    }
}
