using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FurkanKambay.UI
{
    public class CastleHealthUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Castle playerCastle;
        [SerializeField] private Castle   enemyCastle;
        [SerializeField] private Vitality playerVitality;
        [SerializeField] private Vitality enemyVitality;

        [Header("UI")]
        [SerializeField] private Image playerBar;
        [SerializeField] private Image    enemyBar;
        [SerializeField] private TMP_Text playerQueueLabel;
        [SerializeField] private TMP_Text enemyQueueLabel;

        private void Update()
        {
            playerBar.rectTransform.localScale = new Vector3(playerVitality.HealthNormalized, 1, 1);
            enemyBar.rectTransform.localScale  = new Vector3(enemyVitality.HealthNormalized,  1, 1);

            UpdateQueueLabel(playerQueueLabel, playerCastle.SpawnQueueCount);
            UpdateQueueLabel(enemyQueueLabel,  enemyCastle.SpawnQueueCount);
        }

        private static void UpdateQueueLabel(TMP_Text label, int queueCount)
        {
            label.alpha = queueCount == 0 ? 0 : 1;
            label.text  = queueCount.ToString();
        }
    }
}
