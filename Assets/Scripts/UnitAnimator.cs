using System.Collections;
using UnityEngine;

namespace FurkanKambay
{
    public class UnitAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;
        [SerializeField] private Attacker attacker;
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
            spriteRenderer.sprite = attacker.Unit.UnitSO.Sprite;
        }

        private void OnEnable()
        {
            attacker.OnAttackStarted += Attacker_AttackStarted;
            vitality.OnDamageTaken += Vitality_DamageTaken;
            vitality.OnDied += Vitality_Died;
        }

        private void OnDisable()
        {
            attacker.OnAttackStarted -= Attacker_AttackStarted;
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

        private void Anim_ProcAttack() =>
            attacker.ProcAttack();
    }
}
