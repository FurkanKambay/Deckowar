using UnityEngine;
using UnityEngine.UI;

namespace FurkanKambay.UI
{
    public class CastleHealthUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Vitality playerVitality;
        [SerializeField] private Vitality enemyVitality;

        [Header("UI")]
        [SerializeField] private Image playerBar;
        [SerializeField] private Image enemyBar;

        private void Update()
        {
            playerBar.rectTransform.localScale = new Vector3(playerVitality.HealthNormalized, 1, 1);
            enemyBar.rectTransform.localScale  = new Vector3(enemyVitality.HealthNormalized,  1, 1);
        }
    }
}
