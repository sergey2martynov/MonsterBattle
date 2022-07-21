using Player;
using UnityEngine;

namespace CameraFollow
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private PlayerView _playerView;

        private Vector3 _offset = new Vector3(0, 18, -6.4f);

        private void LateUpdate()
        {
            transform.position = _playerView.transform.position + _offset;
        }
    }
}
