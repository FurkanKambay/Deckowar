using UnityEngine;
using UnityEngine.InputSystem;

namespace FurkanKambay.Util
{
    public class GodMode : MonoBehaviour
    {
        [SerializeField] private DeckHolder deckHolder;
        [SerializeField] private Castle     playerCastle;
        [SerializeField] private Castle     enemyCastle;

        private void Update()
        {
            if (Keyboard.current.jKey.wasPressedThisFrame)
                playerCastle.EnqueueSpawnUnit();

            if (Keyboard.current.kKey.wasPressedThisFrame)
                enemyCastle.EnqueueSpawnUnit();

            if (Keyboard.current.dKey.wasPressedThisFrame)
            {
                deckHolder.DiscardHand();
                deckHolder.DrawHand();
            }
        }
    }
}
