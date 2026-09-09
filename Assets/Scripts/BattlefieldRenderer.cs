using UnityEngine;

namespace FK.Deckowar
{
    public sealed class BattlefieldRenderer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Battlefield battlefield;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Header("Config")]
        [SerializeField, Min(1)] private int multiplier = 2;
        [SerializeField, Min(0)] private float height = 2f;

        private void OnEnable()
        {
            ResizeSprite();
        }

        private void ResizeSprite()
        {
            spriteRenderer.drawMode = SpriteDrawMode.Tiled;
            spriteRenderer.size = new Vector2(battlefield.RankCount * multiplier, height);
        }

#if UNITY_EDITOR
        private void OnValidate() => ResizeSprite();
#endif
    }
}
