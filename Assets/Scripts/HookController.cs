using UnityEngine;

namespace MustafaNaqvi
{
    public class HookController : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerMana playerMana;
        [SerializeField] private LayerMask damageableLayer;
        [SerializeField] private Transform hookTransform;
        [SerializeField] private float manaRequired;
        [SerializeField] private float hookSpeed;
        [SerializeField] private float maxDistance;
        [SerializeField] private float pullSpeed;

        internal bool IsHooking;
        private Transform _hookTarget;

        private void Start()
        {
            if (ReferenceEquals(playerController, null) && TryGetComponent<PlayerController>(out var pc))
                playerController = pc;
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(1) && !IsHooking && playerMana.Mana >= manaRequired)
            {
                FireHook();
            }

            if (IsHooking)
            {
                MoveHookToTarget();
            }
            else
            {
                RetractHook();
                PullTarget();
            }
        }

        private void FireHook()
        {
            if (ReferenceEquals(hookTransform, null)) return;
            var direction = Vector2.zero;
            switch (playerController.FacingDirection)
            {
                case FacingDirection.Right:
                    direction = Vector2.right;
                    break;
                case FacingDirection.Left:
                    direction = Vector2.left;
                    break;
                case FacingDirection.Up:
                    direction = Vector2.up;
                    break;
                case FacingDirection.Down:
                    direction = Vector2.down;
                    break;
            }

            var hit = Physics2D.Raycast(hookTransform.position, direction, maxDistance, damageableLayer);

            if (ReferenceEquals(hit.collider, null)) return;
            _hookTarget = hit.transform;
            playerMana.ConsumeMana(manaRequired);
            IsHooking = true;
        }

        private void MoveHookToTarget()
        {
            if (ReferenceEquals(hookTransform, null)) return;
            if (ReferenceEquals(_hookTarget, null))
            {
                IsHooking = false;
                return;
            }

            hookTransform.position =
                Vector2.MoveTowards(hookTransform.position, _hookTarget.position, hookSpeed * Time.deltaTime);

            if (!(Vector2.Distance(hookTransform.position, _hookTarget.position) < 0.1f)) return;
            IsHooking = false;
        }

        private void RetractHook()
        {
            if (ReferenceEquals(hookTransform, null)) return;
            hookTransform.localPosition =
                Vector2.MoveTowards(hookTransform.localPosition, Vector3.up, hookSpeed * Time.deltaTime);

            if (Vector2.Distance(hookTransform.position, transform.position) >= 0.05f) return;
            IsHooking = false;
        }

        private void PullTarget()
        {
            if (ReferenceEquals(_hookTarget, null)) return;
            _hookTarget.position =
                Vector2.MoveTowards(_hookTarget.position, transform.position, pullSpeed * Time.deltaTime);

            if (Vector2.Distance(_hookTarget.position, transform.position) >= 0.05f) return;
            _hookTarget = null;
        }
    }
}