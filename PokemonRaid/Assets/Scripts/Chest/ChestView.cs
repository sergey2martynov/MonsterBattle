using Player;
using UnityEngine;

namespace Chest
{
    public class ChestView : MonoBehaviour
    {
        [SerializeField] private GameObject _egg;
        [SerializeField] private GameObject _chest;

        public GameObject Egg => _egg;
        public GameObject Chest => _chest;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerView playerView))
            {
                playerView.LevelFinish();
            }
        }
    }
}
