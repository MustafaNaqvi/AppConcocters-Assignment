using UnityEngine;

namespace MustafaNaqvi
{
    public class HealthPickup : CollectableObject
    {
        [SerializeField] private float spawnTime;
        [SerializeField] private float healthToAdd;
        private void Start()
        {
            InvokeRepeating(nameof(Spawn), spawnTime, spawnTime);
        }

        protected override void Collect(GameObject collectingObject)
        {
            if (!collectingObject.TryGetComponent<CharacterHealth>(out var health)) return;
            health.AddHealth(healthToAdd);
            if (ReferenceEquals(SpawnedObject, null)) return;
            Destroy(SpawnedObject);
            if (!IsInvoking(nameof(Spawn))) return;
            CancelInvoke(nameof(Spawn));
            InvokeRepeating(nameof(Spawn), spawnTime, spawnTime);
        }
    }
}
