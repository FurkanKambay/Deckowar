using Deckowar.Data;
using Deckowar.Deckbuilding;
using UnityEngine;

namespace Deckowar
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private DeckHolder deckHolder;
        [SerializeField] private BotStrategySO strategySO;

        private int strategyStep;
        private int strategySubstep;
        private CardSO nextCardSO;

        private int StepCount => strategySO.CardBundles.Count;
        private int CurrentSubstepCount => CurrentBundle?.Amount ?? 0;

        private CardBundle CurrentBundle =>
            strategyStep < StepCount ? strategySO.CardBundles[strategyStep] : null;

        private void Start()
        {
            if (!strategySO || StepCount == 0)
                enabled = false;
        }

        private void Update()
        {
            if (CurrentBundle is null)
                return;

            nextCardSO = CurrentBundle.CardSO;

            if (deckHolder.Gold < nextCardSO.Cost)
                return;

            foreach (Card card in deckHolder.Deck.HandPile.ListRO)
            {
                if (card.CardSO != nextCardSO || !deckHolder.TryUseCard(card))
                    continue;

                NextSubstep();
                break;
            }

            deckHolder.DiscardHand();
            deckHolder.DrawHand();
        }

        private void NextSubstep()
        {
            if (strategySubstep == CurrentSubstepCount - 1)
            {
                if (strategyStep == strategySO.CardBundles.Count - 1)
                {
                    // Stay on the last step, repeat it forever
                    strategySubstep = 0;
                    return;
                }

                strategyStep++;
                strategySubstep = 0;
                return;
            }

            strategySubstep++;
        }
    }
}
