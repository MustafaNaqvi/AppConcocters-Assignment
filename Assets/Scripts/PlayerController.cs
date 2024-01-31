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
    }
}