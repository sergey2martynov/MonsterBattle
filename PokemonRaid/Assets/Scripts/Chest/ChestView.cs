using Player;
using UnityEngine;

namespace Chest
{
    public class ChestView : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerView playerView))
            {
                playerView.LevelFinish();
            }
        }
    }
}
