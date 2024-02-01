using UnityEngine;

namespace MustafaNaqvi
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer playerSprite;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private Rigidbody2D playerRigidBody;
        [SerializeField] private MeleeAttack meleeAttack;
        [SerializeField] private float moveSpeed;

        private static readonly int Running = Animator.StringToHash("Running");

        internal FacingDirection FacingDirection;
        private float _horizontal, _vertical;
        private bool _keyCollected;

        private void Start()
        {
            if (ReferenceEquals(playerSprite, null) && TryGetComponent<SpriteRenderer>(out var spriteRenderer))
                playerSprite = spriteRenderer;

            if (ReferenceEquals(playerAnimator, null) && TryGetComponent<Animator>(out var animator))
                playerAnimator = animator;

            if (ReferenceEquals(playerRigidBody, null) && TryGetComponent<Rigidbody2D>(out var rigidBody2D))
                playerRigidBody = rigidBody2D;

            if (ReferenceEquals(meleeAttack, null) && TryGetComponent<MeleeAttack>(out var ma))
                meleeAttack = ma;
            KeyPickup.KeyCollected += () => _keyCollected = true;
        }

        private void Update()
        {
            HandleInput();
            HandleMovement();
            HandleRotation();
        }

        private void HandleInput()
        {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");
        }

        private void HandleMovement()
        {
            var movement = new Vector3(_horizontal, _vertical, 0f).normalized;
            if (ReferenceEquals(meleeAttack, null)) return;
            if (Equals(movement, Vector3.zero) || meleeAttack.Attacking)
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

            FacingDirection = _vertical switch
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

            playerSprite.flipX = FacingDirection switch
            {
                FacingDirection.Right => false,
                FacingDirection.Left => true,
                FacingDirection.Up => false,
                FacingDirection.Down => false,
                _ => playerSprite.flipX
            };
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.collider.CompareTag("Door")) return;
            if (!_keyCollected) return;
            // Game Complete
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