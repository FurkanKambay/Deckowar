using FurkanKambay.Deckbuilding;
using UnityEngine;

namespace FurkanKambay
{
    public class DeckHolder : MonoBehaviour
    {
        [SerializeField] private DeckConfigSO deckConfigSO;

        private Deck deck;

        private void Awake()
        {
            deck = new Deck(deckConfigSO);
            deck.FillUpHand();
        }

        [ContextMenu("Fill Up Hand")]
        private void FillUpHand() => deck.FillUpHand();

        [ContextMenu("Discard Hand")]
        private void DiscardHand() => deck.DiscardHand();
    }
}
