using UnityEngine;
using UnityEngine.AI;

namespace MustafaNaqvi
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer enemySprite;

        private PlayerController _playerController;
        private static readonly int Running = Animator.StringToHash("Running");

        private void Start()
        {
            _playerController ??= FindObjectOfType<PlayerController>();

            if (ReferenceEquals(agent, null) && TryGetComponent<NavMeshAgent>(out var ag))
                agent = ag;

            if (ReferenceEquals(agent, null)) return;
            agent.updateRotation = agent.updateUpAxis = false;
        }

        private void LateUpdate()
        {
            HandleMovement();
            HandleRotation();
        }

        private void HandleMovement()
        {
            if (ReferenceEquals(_playerController, null)) return;
            if (ReferenceEquals(agent, null)) return;
            if (ReferenceEquals(animator, null)) return;
            agent.SetDestination(_playerController.transform.position + Vector3.up);
            animator.SetBool(Running, agent.remainingDistance > agent.stoppingDistance);
        }

        private void HandleRotation()
        {
            if (ReferenceEquals(enemySprite, null)) return;
            enemySprite.flipX = _playerController.transform.position.x < transform.position.x;
        }
    }
}