using Deckowar.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Deckowar.Util
{
    public class GodMode : MonoBehaviour
    {
        [SerializeField] private DeckHolder deckHolder;
        [SerializeField] private Castle playerCastle;
        [SerializeField] private Castle enemyCastle;

        [Header("Spawned Units")]
        [SerializeField] private UnitSO unitSO;

        private void Update()
        {
            if (Keyboard.current.jKey.wasPressedThisFrame)
                playerCastle.EnqueueSpawnUnit(unitSO);

            if (Keyboard.current.kKey.wasPressedThisFrame)
                enemyCastle.EnqueueSpawnUnit(unitSO);

            if (Keyboard.current.dKey.wasPressedThisFrame)
            {
                deckHolder.DiscardHand();
                deckHolder.DrawHand();
            }
        }
    }
}
