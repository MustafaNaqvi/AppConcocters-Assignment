using UnityEngine;

namespace MustafaNaqvi
{
    public class EnemyRangedAttack : MonoBehaviour
    {
        [SerializeField] private GameObject rangedAttackObject;
        [SerializeField] private Animator animator;
        [SerializeField] private float damageToGive;
        [SerializeField] private float rangedAttackSpeed;
        [SerializeField] private float bufferTimeBetweenAttacks;

        private EnemyController _enemyController;
        private GameObject _spawnedRangedAttackObject;
        private Vector3 _playerPosition;
        private float _elapsedTime;
        private bool _isAttacking;
        private static readonly int AttackString = Animator.StringToHash("Attack");

        private void Start()
        {
            if (ReferenceEquals(_enemyController, null) && TryGetComponent<EnemyController>(out var enemyController))
                _enemyController = enemyController;

            if (ReferenceEquals(_enemyController, null)) return;

            if (ReferenceEquals(_enemyController.EnemyHealth, null)) return;

            _enemyController.EnemyHealth.death += StopAttack;
        }

        private void LateUpdate()
        {
            if (ReferenceEquals(_enemyController.EnemyHealth, null)) return;
            if (_enemyController.EnemyHealth.health <= 0) return;

            if (_elapsedTime < bufferTimeBetweenAttacks)
            {
                _elapsedTime += Time.deltaTime;
                return;
            }

            if (_isAttacking)
            {
                MoveRangedAttackObject();
                return;
            }

            if (!_enemyController.ReachedPlayer) return;

            PlayAttackAnimation();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void PlayAttackAnimation()
        {
            if (ReferenceEquals(animator, null)) return;
            if (_isAttacking) return;

            _isAttacking = true;
            animator.SetTrigger(AttackString);
            Invoke(nameof(Attack), animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * 0.5f);
        }

        private void Attack()
        {
            if (ReferenceEquals(rangedAttackObject, null)) return;
            if (ReferenceEquals(animator, null)) return;
            if (!ReferenceEquals(_spawnedRangedAttackObject, null)) return;
            if (ReferenceEquals(_enemyController.PlayerController, null)) return;

            animator.ResetTrigger(AttackString);
            _playerPosition = _enemyController.PlayerController.transform.position + Vector3.up;
            _spawnedRangedAttackObject = Instantiate(rangedAttackObject, transform.position, Quaternion.identity);
        }

        private void MoveRangedAttackObject()
        {
            if (ReferenceEquals(_enemyController.PlayerController, null)) return;
            if (ReferenceEquals(_spawnedRangedAttackObject, null)) return;

            _spawnedRangedAttackObject.transform.position =
                Vector3.MoveTowards(_spawnedRangedAttackObject.transform.position,
                    _playerPosition, rangedAttackSpeed * Time.deltaTime);

            if (Vector3.Distance(_spawnedRangedAttackObject.transform.position, _playerPosition) > 0.1f)
                return;

            StopAttack();
        }

        private void StopAttack()
        {
            _isAttacking = false;
            if (ReferenceEquals(_spawnedRangedAttackObject, null)) return;
            GiveDamage();
            Destroy(_spawnedRangedAttackObject);
            _spawnedRangedAttackObject = null;
            _elapsedTime = 0f;
        }

        private void GiveDamage()
        {
            if (ReferenceEquals(_enemyController.EnemyHealth, null)) return;
            if (_enemyController.EnemyHealth.health <= 0f) return;
            if (ReferenceEquals(_enemyController.PlayerController, null)) return;
            if (ReferenceEquals(_spawnedRangedAttackObject, null)) return;

            if (Vector3.Distance(_enemyController.PlayerController.transform.position, _playerPosition) > 1.05f) return;

            if (!_enemyController.PlayerController.TryGetComponent<CharacterHealth>(out var playerHealth)) return;

            playerHealth.Damage(damageToGive);
        }
    }
}