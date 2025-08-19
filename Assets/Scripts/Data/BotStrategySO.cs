using System.Collections.Generic;
using FurkanKambay.Deckbuilding;
using UnityEngine;

namespace FurkanKambay.Data
{
    [CreateAssetMenu]
    public class BotStrategySO : ScriptableObject
    {
        [SerializeField] private DeckConfigSO.Set[] cardSets;

        public IReadOnlyList<DeckConfigSO.Set> CardSets => cardSets;
    }
}
