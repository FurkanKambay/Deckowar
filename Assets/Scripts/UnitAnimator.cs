using System.Collections;
using Deckowar.Data;
using UnityEngine;

namespace Deckowar
{
    public class UnitAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;
        [SerializeField] private Vitality vitality;

        [Header("Config")]
        [SerializeField] private float hurtDuration;

        private MaterialPropertyBlock propertyBlock;

        private static readonly int AnimAttack = Animator.StringToHash("attack");
        private static readonly int ShaderHurt = Shader.PropertyToID("_Hurt");

        private void Awake()
        {
            propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetInt(ShaderHurt, 0);
            spriteRenderer.SetPropertyBlock(propertyBlock);
        }

        private void Start()
        {
            // TODO: assign unit sprite
            // spriteRenderer.sprite = unit.UnitSO.Sprite;
        }

        private void OnEnable()
        {
            vitality.OnDamageTaken += Vitality_DamageTaken;
            vitality.OnDied += Vitality_Died;
        }

        private void OnDisable()
        {
            vitality.OnDamageTaken -= Vitality_DamageTaken;
            vitality.OnDied -= Vitality_Died;
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
            Destroy(vitality.gameObject, t: 0f);
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
