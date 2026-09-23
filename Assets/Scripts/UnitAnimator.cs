using System.Collections;
using FK.Deckowar.Core;
using UnityEngine;
using UnityEngine.Assertions;

namespace FK.Deckowar
{
    public class UnitAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Unit unit;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;

        [Header("Config")]
        [SerializeField] private float hurtDuration;

        private MaterialPropertyBlock propertyBlock;

        private static readonly int animAttack = Animator.StringToHash("attack");
        private static readonly int shaderHurt = Shader.PropertyToID("_Hurt");

        private void Awake()
        {
            Assert.IsNotNull(spriteRenderer);
            Assert.IsNotNull(animator);

            propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetInt(shaderHurt, 0);
            spriteRenderer.SetPropertyBlock(propertyBlock);
        }

        private void Start()
        {
            Assert.IsNotNull(unit);

            unit.Vitality.OnDamageTaken += Vitality_DamageTaken;
            unit.Vitality.OnDied += Vitality_Died;

            spriteRenderer.sprite = unit.UnitAsset.Sprite;
            spriteRenderer.flipX = unit.Faction == Faction.Enemy;
        }

        private void OnDestroy()
        {
            Assert.IsNotNull(unit);

            unit.Vitality.OnDamageTaken -= Vitality_DamageTaken;
            unit.Vitality.OnDied -= Vitality_Died;
        }

        private void Vitality_DamageTaken() =>
            StartCoroutine(GetHurt());

        private IEnumerator GetHurt()
        {
            propertyBlock.SetInt(shaderHurt, 1);
            spriteRenderer.SetPropertyBlock(propertyBlock);
            yield return new WaitForSeconds(hurtDuration);

            propertyBlock.SetInt(shaderHurt, 0);
            spriteRenderer.SetPropertyBlock(propertyBlock);
        }

        private void Vitality_Died()
        {
            // Destroy(Unit.Vitality.gameObject, t: 0f);
        }

        private void Attacker_AttackStarted()
        {
            animator.SetTrigger(animAttack);
        }

        private void Anim_ProcAttack()
        {
            // TODO: send attack proc signal
        }
    }
}
