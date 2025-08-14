using UnityEngine;

namespace FurkanKambay
{
    public class UnitAttackVisual : MonoBehaviour
    {
        [SerializeField] private Attacker attacker;
        [SerializeField] private Animator animator;

        private static readonly int AnimAttack = Animator.StringToHash("attack");

        private void OnEnable()  => attacker.OnAttackStarted += Attacker_AttackStarted;
        private void OnDisable() => attacker.OnAttackStarted -= Attacker_AttackStarted;

        private void Attacker_AttackStarted()
        {
            animator.SetTrigger(AnimAttack);
        }

        private void Anim_ProcAttack() =>
            attacker.ProcAttack();
    }
}
