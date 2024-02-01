using UnityEngine;
using UnityEngine.AI;

namespace MustafaNaqvi
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer enemySprite;
        [SerializeField] private GameObject onDeathVFX;

        internal PlayerController PlayerController;
        internal CharacterHealth EnemyHealth;
        internal bool ReachedPlayer;

        private static readonly int Running = Animator.StringToHash("Running");

        private void OnEnable()
        {
            if (ReferenceEquals(EnemyHealth, null) && TryGetComponent<CharacterHealth>(out var enemyHealth))
                EnemyHealth = enemyHealth;
            if (ReferenceEquals(EnemyHealth, null)) return;
            EnemyHealth.death += OnDeath;
        }

        private void OnDisable()
        {
            if (ReferenceEquals(EnemyHealth, null)) return;
            EnemyHealth.death -= OnDeath;
        }

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

        private void OnDeath()
        {
            Invoke(nameof(Die), 1f);
        }

        private void Die()
        {
            Instantiate(onDeathVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}