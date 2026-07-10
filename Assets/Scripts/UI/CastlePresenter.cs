using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FK.Deckowar.UI
{
    public class CastlePresenter : MonoBehaviour
    {
        [SerializeField] private Castle castle;

        [Header("UI")]
        [SerializeField] private Image bar;
        [SerializeField] private Image spawnRadial;
        [SerializeField] private TMP_Text queueLabel;
        [SerializeField] private TMP_Text goldLabel;

        public void Update()
        {
            bar.rectTransform.localScale = new Vector3(castle.Vitality.HealthNormalized, 1, 1);

            // spawnRadial.fillAmount = castle.ProgressUntilNextSpawn;
            queueLabel.alpha = castle.SpawnQueueCount == 0 ? 0 : 1;
            queueLabel.text = castle.SpawnQueueCount.ToString();
            goldLabel.text = $"{castle.Gold:N0}";
        }
    }
}
