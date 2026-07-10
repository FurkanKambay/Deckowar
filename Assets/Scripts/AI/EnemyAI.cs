using FK.Deckowar.Data;
using FK.Deckowar.Deckbuilding;
using UnityEngine;

namespace FK.Deckowar.AI
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private DeckHolder deckHolder;
        [SerializeField] private BotStrategyAsset strategySO;

        private int strategyStep;
        private int strategySubstep;
        private CardAsset nextCardAsset;

        private int StepCount => strategySO.CardBundles.Count;
        private int CurrentSubstepCount => CurrentBundle.Amount;

        private CardBundle CurrentBundle =>
            strategyStep < StepCount ? strategySO.CardBundles[strategyStep] : default;

        private void Start()
        {
            if (!strategySO || StepCount == 0)
                enabled = false;
        }

        private void Update()
        {
            if (!CurrentBundle.IsValid)
                return;

            nextCardAsset = CurrentBundle.CardAsset;

            if (deckHolder.Castle.Gold < nextCardAsset.Cost)
                return;

            foreach (Card card in deckHolder.Deck.HandPile.ListRO)
            {
                if (card.CardAsset != nextCardAsset || !deckHolder.TryUseCard(card))
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
