using UnityEngine;

namespace MustafaNaqvi
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private GameObject guide;

        private void Start()
        {
            KeyPickup.KeyCollected += () => guide.gameObject.SetActive(true);
        }
    }
}