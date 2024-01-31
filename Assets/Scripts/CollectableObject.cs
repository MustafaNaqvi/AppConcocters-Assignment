using UnityEngine;

namespace MustafaNaqvi
{
    public abstract class CollectableObject : MonoBehaviour
    {
        [SerializeField] protected Transform spawnPoint;
        [SerializeField] protected GameObject prefab;

        protected GameObject spawnedObject;

        protected void Spawn()
        {
            if (spawnPoint.childCount > 0) return;
            spawnedObject = Instantiate(prefab, spawnPoint);
        }

        protected abstract void Collect(GameObject collectingObject);

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (ReferenceEquals(spawnedObject, null)) return;
            Collect(other.gameObject);
        }
    }
}