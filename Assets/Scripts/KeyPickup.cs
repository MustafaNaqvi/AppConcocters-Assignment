using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MustafaNaqvi
{
    public class KeyPickup : CollectableObject
    {
        [SerializeField] private List<Transform> repositionPoints = new();

        public static Action KeyCollected;

        private void Start()
        {
            if (repositionPoints.Count > 0)
                transform.position = repositionPoints[Random.Range(0, repositionPoints.Count)].position;
            Spawn();
        }

        protected override void Collect(GameObject collectingObject)
        {
            if (ReferenceEquals(SpawnedObject, null)) return;
            KeyCollected?.Invoke();
            Destroy(SpawnedObject);
        }

        private void OnDestroy()
        {
            KeyCollected = null;
        }
    }
}