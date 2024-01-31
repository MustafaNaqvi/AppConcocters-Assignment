using UnityEngine;

namespace MustafaNaqvi
{
    public abstract class CollectableObject : MonoBehaviour
    {
        [SerializeField] protected Transform spawnPoint;
        [SerializeField] protected GameObject prefab;

        protected GameObject SpawnedObject;

        protected void Spawn()
        {
            if (spawnPoint.childCount > 0) return;
            SpawnedObject = Instantiate(prefab, spawnPoint);
        }

        protected abstract void Collect(GameObject collectingObject);

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (ReferenceEquals(SpawnedObject, null)) return;
            Collect(other.gameObject);
        }
    }
}