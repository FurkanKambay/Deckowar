using System.Collections;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FK.Deckowar.UI
{
    public class CastleHUD : MonoBehaviour
    {
        [SerializeField] private Castle castle;

        [Header("UI")]
        [SerializeField] private Image bar;

        [Header("Spawn Queue")]
        [SerializeField] private Image spawnRadial;
        [SerializeField] private TMP_Text queueLabel;

        [Header("Gold")]
        [SerializeField] private Image goldBackground;
        [SerializeField] private TMP_Text goldLabel;
        [SerializeField] private float goldScaleSpeed;

        public void Init(Castle castle)
        {
            this.castle = castle;
        }

        private void OnEnable()
        {
            if (!castle) return;
            castle.PropertyChanged += Castle_PropertyChanged;
        }

        private void OnDisable()
        {
            if (!castle) return;
            castle.PropertyChanged -= Castle_PropertyChanged;
        }

        public void Update()
        {
            if (!castle)
                return;

            bar.rectTransform.localScale = new Vector3(castle.Vitality.HealthNormalized, 1, 1);

            // spawnRadial.fillAmount = castle.ProgressUntilNextSpawn;
            queueLabel.alpha = castle.SpawnQueueCount == 0 ? 0 : 1;
            queueLabel.text = castle.SpawnQueueCount.ToString();
        }

        private void Castle_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            goldLabel.text = $"{castle.Gold:N0}";

            StopCoroutine(DoScaleGoldBackground());
            StartCoroutine(DoScaleGoldBackground());
        }

        private IEnumerator DoScaleGoldBackground()
        {
            Vector3 initial = Vector3.one * 1f;
            Vector3 target = Vector3.one * 1.5f;

            RectTransform gold = goldBackground.rectTransform;
            while (gold.localScale != target)
            {
                float delta = goldScaleSpeed * Time.deltaTime;
                gold.localScale = Vector3.MoveTowards(gold.localScale, target, delta);
                yield return null;
            }

            while (gold.localScale != initial)
            {
                float delta = goldScaleSpeed * Time.deltaTime;
                gold.localScale = Vector3.MoveTowards(gold.localScale, initial, delta);
                yield return null;
            }
        }
    }
}
