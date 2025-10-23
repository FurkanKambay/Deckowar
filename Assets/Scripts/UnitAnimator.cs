using System.Collections;
using Deckowar.Core;
using UnityEngine;
using UnityEngine.Assertions;

namespace Deckowar
{
    public class UnitAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;

        [Header("Config")]
        [SerializeField] private float hurtDuration;

        public Unit Unit { get; private set; }

        private MaterialPropertyBlock propertyBlock;

        private static readonly int AnimAttack = Animator.StringToHash("attack");
        private static readonly int ShaderHurt = Shader.PropertyToID("_Hurt");

        private void Awake()
        {
            Assert.IsNotNull(spriteRenderer);
            Assert.IsNull(animator);

            propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetInt(ShaderHurt, 0);
            spriteRenderer.SetPropertyBlock(propertyBlock);
        }

        public void Init(Unit unit)
        {
            Unit = unit;
        }

        private void Start()
        {
            Assert.IsNotNull(Unit);

            Unit.Vitality.OnDamageTaken += Vitality_DamageTaken;
            Unit.Vitality.OnDied += Vitality_Died;

            spriteRenderer.sprite = Unit.UnitSO.Sprite;
        }

        private void OnDestroy()
        {
            Assert.IsNotNull(Unit);

            Unit.Vitality.OnDamageTaken -= Vitality_DamageTaken;
            Unit.Vitality.OnDied -= Vitality_Died;
        }

        private void Vitality_DamageTaken() =>
            StartCoroutine(GetHurt());

        private IEnumerator GetHurt()
        {
            propertyBlock.SetInt(ShaderHurt, 1);
            spriteRenderer.SetPropertyBlock(propertyBlock);
            yield return new WaitForSeconds(hurtDuration);

            propertyBlock.SetInt(ShaderHurt, 0);
            spriteRenderer.SetPropertyBlock(propertyBlock);
        }

        private void Vitality_Died()
        {
            // Destroy(vitality.gameObject, t: 0f);
        }

        private void Attacker_AttackStarted()
        {
            animator.SetTrigger(AnimAttack);
        }

        private void Anim_ProcAttack()
        {
            // TODO: send attack proc signal
        }
    }
}
