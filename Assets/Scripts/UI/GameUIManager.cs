using UnityEngine;

namespace FK.Deckowar.UI
{
    public class GameUIManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private HandVisual handVisual;
        [SerializeField] private CastleHUD playerHUD;
        [SerializeField] private CastleHUD enemyHUD;

        [Header("Simulation")]
        [SerializeField] private Castle playerCastle;
        [SerializeField] private Castle enemyCastle;
        [SerializeField] private DeckHolder playerDeckHolder;

        private void OnEnable()
        {
            playerHUD.Init(playerCastle);
            enemyHUD.Init(enemyCastle);
            handVisual.Init(playerDeckHolder);
        }
    }
}
