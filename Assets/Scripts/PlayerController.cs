using UnityEngine;

namespace MustafaNaqvi
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer playerSprite;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private Rigidbody2D playerRigidBody;
        [SerializeField] private float moveSpeed;

        private static readonly int Running = Animator.StringToHash("Running");

        private FacingDirection _facingDirection;
        private float _horizontal, _vertical;

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
        }

        private void HandleInput()
        {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");
        }

        private void HandleMovement()
        {
            var movement = new Vector3(_horizontal, _vertical, 0f).normalized;
            if (Equals(movement, Vector3.zero))
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
    }

    public enum FacingDirection
    {
        Right,
        Left,
        Up,
        Down
    }
}