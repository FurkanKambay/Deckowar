using UnityEngine;

namespace FK.Deckowar
{
    public class CastleRenderer : MonoBehaviour
    {
        [SerializeField] private Castle castle;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer.sprite = castle.StatsAsset.Sprite;
        }
    }
}
