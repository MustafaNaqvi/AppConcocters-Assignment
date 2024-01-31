using UnityEngine;

namespace MustafaNaqvi
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private float spawnTime;

        private void Start()
        {
            InvokeRepeating(nameof(Spawn), spawnTime, spawnTime);
        }

        private void Spawn()
        {
            if (spawnPoint.childCount > 0) return;
            Instantiate(enemyPrefab, spawnPoint);
        }
    }
}