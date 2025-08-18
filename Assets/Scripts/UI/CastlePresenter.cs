using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FurkanKambay.UI
{
    public class CastlePresenter : MonoBehaviour
    {
        [SerializeField] private CastleHUDInfo playerCastleInfo;
        [SerializeField] private CastleHUDInfo enemyCastleInfo;

        private void Update()
        {
            playerCastleInfo.UpdateUI();
            enemyCastleInfo.UpdateUI();
        }

        [Serializable]
        private class CastleHUDInfo
        {
            [SerializeField] private Castle castle;

            [Header("UI")]
            [SerializeField] private Image bar;
            [SerializeField] private Image    queueRadial;
            [SerializeField] private TMP_Text queueLabel;

            public void UpdateUI()
            {
                bar.rectTransform.localScale = new Vector3(castle.Vitality.HealthNormalized, 1, 1);

                queueRadial.fillAmount = castle.ProgressUntilNextSpawn;
                queueLabel.alpha       = castle.SpawnQueueCount == 0 ? 0 : 1;
                queueLabel.text        = castle.SpawnQueueCount.ToString();
            }
        }
    }
}
