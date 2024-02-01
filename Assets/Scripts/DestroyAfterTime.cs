using UnityEngine;

namespace MustafaNaqvi
{
    public class DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] private float waitTime;

        private void Start()
        {
            Destroy(gameObject, waitTime);
        }
    }
}