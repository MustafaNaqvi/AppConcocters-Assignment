using UnityEngine;

namespace MustafaNaqvi
{
    public class MeleeAttack : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private BoxCollider2D weaponCollider;

        private static readonly int AttackHorizontal = Animator.StringToHash("Attack_Horizontal");
        private static readonly int AttackUp = Animator.StringToHash("Attack_Up");
        private static readonly int AttackDown = Animator.StringToHash("Attack_Down");

        internal bool Attacking;
        public float damageToGive;

        private void Start()
        {
            if (ReferenceEquals(playerController, null) && TryGetComponent<PlayerController>(out var pc))
                playerController = pc;

            if (ReferenceEquals(playerAnimator, null) && TryGetComponent<Animator>(out var animator))
                playerAnimator = animator;
        }

        private void Update()
        {
            HandleMeleeAttack();
        }

        private void HandleMeleeAttack()
        {
            if (ReferenceEquals(playerController, null)) return;
            if (!Input.GetMouseButtonUp(0) || Attacking) return;

            Attacking = true;

            switch (playerController.FacingDirection)
            {
                case FacingDirection.Right or FacingDirection.Left:
                    PlayAttackAnimation(AttackHorizontal);
                    break;

                case FacingDirection.Up:
                    PlayAttackAnimation(AttackUp);
                    break;

                case FacingDirection.Down:
                    PlayAttackAnimation(AttackDown);
                    break;
            }

            HandleWeaponCollider();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void PlayAttackAnimation(int id)
        {
            playerAnimator.ResetTrigger(AttackHorizontal);
            playerAnimator.ResetTrigger(AttackUp);
            playerAnimator.ResetTrigger(AttackDown);
            playerAnimator.SetTrigger(id);

            Invoke(nameof(ResetAttacking), playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        }

        private void HandleWeaponCollider()
        {
            if (ReferenceEquals(playerController, null)) return;
            if (ReferenceEquals(weaponCollider, null)) return;

            /*
            var offset = weaponCollider.offset;
            switch (playerController.FacingDirection)
            {
                case FacingDirection.Right:
                    offset.x += 0.25f;
                    break;

                case FacingDirection.Left:
                    offset.x -= 0.25f;
                    break;

                case FacingDirection.Up:
                    offset.y += 0.25f;
                    break;

                case FacingDirection.Down:
                    offset.y -= 0.25f;
                    break;
            }

            weaponCollider.offset = offset;
            */
            weaponCollider.size = Vector2.one * 1.25f;
        }

        private void ResetAttacking()
        {
            Attacking = false;
            if (ReferenceEquals(weaponCollider, null)) return;
            weaponCollider.offset = Vector2.zero;
            weaponCollider.size = Vector2.one * 0.25f;
        }

        private void GiveDamage(CharacterHealth health)
        {
            if (ReferenceEquals(health, null)) return;
            health.Damage(damageToGive);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.collider.CompareTag("Damageable")) return;
            if (!ReferenceEquals(other.otherCollider, weaponCollider)) return;
            if (!other.collider.TryGetComponent<CharacterHealth>(out var health)) return;
            GiveDamage(health);
        }
    }
}