using UnityEngine;
using UnityEngine.AI;

namespace MustafaNaqvi
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer enemySprite;

        internal PlayerController PlayerController;
        internal bool ReachedPlayer;

        private static readonly int Running = Animator.StringToHash("Running");

        private void Start()
        {
            PlayerController ??= FindObjectOfType<PlayerController>();

            if (ReferenceEquals(agent, null) && TryGetComponent<NavMeshAgent>(out var ag))
                agent = ag;

            if (ReferenceEquals(agent, null)) return;
            agent.updateRotation = agent.updateUpAxis = false;
        }

        private void Update()
        {
            HandleMovement();
            HandleRotation();
        }

        private void HandleMovement()
        {
            if (ReferenceEquals(PlayerController, null)) return;
            if (ReferenceEquals(agent, null)) return;
            if (ReferenceEquals(animator, null)) return;
            agent.SetDestination(PlayerController.transform.position + Vector3.up);
            if (agent.remainingDistance <= 0f) return;
            ReachedPlayer = agent.remainingDistance < agent.stoppingDistance;
            animator.SetBool(Running, !ReachedPlayer);
        }

        private void HandleRotation()
        {
            if (ReferenceEquals(enemySprite, null)) return;
            enemySprite.flipX = PlayerController.transform.position.x < transform.position.x;
        }
    }
}