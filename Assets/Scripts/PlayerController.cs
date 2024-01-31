using UnityEngine;

namespace MustafaNaqvi
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer playerSprite;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private Rigidbody2D playerRigidBody;
        [SerializeField] private BoxCollider2D weaponCollider;
        [SerializeField] private float moveSpeed;

        private static readonly int Running = Animator.StringToHash("Running");
        private static readonly int AttackHorizontal = Animator.StringToHash("Attack_Horizontal");
        private static readonly int AttackUp = Animator.StringToHash("Attack_Up");
        private static readonly int AttackDown = Animator.StringToHash("Attack_Down");

        private FacingDirection _facingDirection;
        private float _horizontal, _vertical;
        private bool _attacking;

        private void Start()
        {
            if (ReferenceEquals(playerSprite, null) && TryGetComponent<SpriteRenderer>(out var spriteRenderer))
                playerSprite = spriteRenderer;

            if (ReferenceEquals(playerAnimator, null) && TryGetComponent<Animator>(out var animator))
                playerAnimator = animator;

            if (ReferenceEquals(playerRigidBody, null) && TryGetComponent<Rigidbody2D>(out var rigidBody2D))
                playerRigidBody = rigidBody2D;
        }

        private void Update()
        {
            HandleInput();
            HandleMovement();
            HandleRotation();
            HandleMeleeAttack();
        }

        private void HandleInput()
        {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");
        }

        private void HandleMovement()
        {
            var movement = new Vector3(_horizontal, _vertical, 0f).normalized;
            if (Equals(movement, Vector3.zero) || _attacking)
            {
                playerAnimator.SetBool(Running, false);
                return;
            }

            playerRigidBody.MovePosition(transform.position + movement * (moveSpeed + Time.deltaTime));
            playerAnimator.SetBool(Running, true);
        }

        private void HandleRotation()
        {
            if (_vertical.Equals(0f) && _horizontal.Equals(0)) return;

            _facingDirection = _vertical switch
            {
                0f when _horizontal > 0f => FacingDirection.Right,
                0f when _horizontal < 0f => FacingDirection.Left,
                > 0f when _horizontal > 0f => FacingDirection.Right,
                > 0f when _horizontal < 0f => FacingDirection.Left,
                > 0f when _horizontal.Equals(0f) => FacingDirection.Up,
                < 0f when _horizontal > 0f => FacingDirection.Right,
                < 0f when _horizontal < 0f => FacingDirection.Left,
                < 0f when _horizontal.Equals(0f) => FacingDirection.Down,
                _ => FacingDirection.Right
            };

            playerSprite.flipX = _facingDirection switch
            {
                FacingDirection.Right => false,
                FacingDirection.Left => true,
                FacingDirection.Up => false,
                FacingDirection.Down => false,
                _ => playerSprite.flipX
            };
        }

        private void HandleMeleeAttack()
        {
            if (!Input.GetKeyUp(KeyCode.Space) || _attacking) return;

            _attacking = true;

            switch (_facingDirection)
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
            if (ReferenceEquals(weaponCollider, null)) return;

            var offset = weaponCollider.offset;
            switch (_facingDirection)
            {
                case FacingDirection.Right:
                    offset.x += 0.5f;
                    break;

                case FacingDirection.Left:
                    offset.x -= 0.5f;
                    break;

                case FacingDirection.Up:
                    offset.y += 0.5f;
                    break;

                case FacingDirection.Down:
                    offset.y -= 0.5f;
                    break;
            }

            weaponCollider.offset = offset;
            weaponCollider.size = Vector2.one * 0.75f;
        }

        private void ResetAttacking()
        {
            _attacking = false;
            if (ReferenceEquals(weaponCollider, null)) return;
            weaponCollider.offset = Vector2.zero;
            weaponCollider.size = Vector2.one * 0.25f;
        }

        private void GiveDamage()
        {
            // Give Damage
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.collider.CompareTag("Damageable")) return;
            if (!ReferenceEquals(other.otherCollider, weaponCollider)) return;
            GiveDamage();
        }
    }

    public enum FacingDirection
    {
        Right,
        Left,
        Up,
        Down
    }
}