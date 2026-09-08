using FK.Deckowar.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using Vertx.Attributes;

namespace FK.Deckowar.Debug
{
    public class GodMode : MonoBehaviour
    {
        [SerializeField] private DeckHolder deckHolder;
        [SerializeField] private Castle playerCastle;
        [SerializeField] private Castle enemyCastle;

        [Header("Config")]
        [HelpBox("J = spawn for Player\nK = spawn for Enemy")]
        [SerializeField] private UnitAsset unitAsset;

        private void Update()
        {
            if (Keyboard.current.jKey.wasPressedThisFrame)
                playerCastle.EnqueueSpawnUnit(unitAsset);

            if (Keyboard.current.kKey.wasPressedThisFrame)
                enemyCastle.EnqueueSpawnUnit(unitAsset);

            if (Keyboard.current.dKey.wasPressedThisFrame)
            {
                deckHolder.DiscardHand();
                deckHolder.DrawHand();
            }
        }
    }
}
