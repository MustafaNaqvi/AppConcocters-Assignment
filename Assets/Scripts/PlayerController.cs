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
            HandleMovementInput();
        }

        private void HandleMovementInput()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            var movement = new Vector3(horizontal, vertical, 0f);
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