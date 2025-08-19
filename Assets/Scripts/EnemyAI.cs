using FurkanKambay.Data;
using UnityEngine;

namespace FurkanKambay
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private DeckHolder deckHolder;
        [SerializeField] private BotStrategySO strategySO;

        private int strategyStep;
        private int strategySubstep;

        private void Start()
        {
            if (!strategySO || strategySO.CardSets.Count == 0)
                enabled = false;
        }

        private void Update()
        {
        }
    }
}
