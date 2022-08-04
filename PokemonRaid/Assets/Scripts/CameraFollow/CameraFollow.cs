using Arena;
using DG.Tweening;
using Player;
using Shop;
using UnityEngine;
using UnityEngine.Serialization;

namespace CameraFollow
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private ShopView _shopView;
        [SerializeField] private Vector3 _vector3;
        [SerializeField] private Vector3 _offsetForArena;
        [SerializeField] private ArenaView _arenaView;

        private bool _isCanAddOffset = true;
        private Vector3 _offset;

        private void Start()
        {
            _offset = _vector3;
            _shopView.StartButtonPressed += MoveCameraToStart;
            _arenaView.PlayerTriggered += MoveCameraToArena;
        }

        private void OnDestroy()
        {
            _shopView.StartButtonPressed -= MoveCameraToStart;
            _arenaView.PlayerTriggered -= MoveCameraToArena;
        }


        private void LateUpdate()
        {
            if (_isCanAddOffset)
                transform.position = _playerView.transform.position + _offset;
        }

        private void MoveCameraToStart()
        {
            _isCanAddOffset = false;

            transform.DORotate(new Vector3(50, 0, 0), 1);

            transform.DOMove(new Vector3(0, 15, 4.18f), 2).OnComplete(() =>
            {
                _offset = new Vector3(0, 15, -2.5f);

                _isCanAddOffset = true;
            });
        }
        
        private void MoveCameraToArena()
        {
            _isCanAddOffset = false;

            transform.DORotate(new Vector3(41, 308, 0), 4);

            var pos = _arenaView.transform.position + _offsetForArena;

            transform.DOMove(pos, 4);
        }
    }
}