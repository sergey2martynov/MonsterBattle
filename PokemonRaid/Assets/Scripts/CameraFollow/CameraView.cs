using Arena;
using DG.Tweening;
using Player;
using Shop;
using UnityEngine;

namespace CameraFollow
{
    public class CameraView : MonoBehaviour
    {
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private ShopView _shopView;
        [SerializeField] private Vector3 _vector3;
        [SerializeField] private Vector3 _offsetForArena;
        [SerializeField] private ArenaMenuView _arenaMenuView;
        [SerializeField] private ParticleSystem _particleSystem;
        private ArenaView _arenaView;

        private bool _isCanAddOffset = true;
        private Vector3 _offset;

        private void Start()
        {
            _offset = _vector3;
            _shopView.StartButtonPressed += MoveCameraToStart;
        }

        private void OnDestroy()
        {
            _shopView.StartButtonPressed -= MoveCameraToStart;
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
        
        public void MoveCameraToArena()
        {
            _isCanAddOffset = false;

            transform.DORotate(new Vector3(41, 308, 0), 4);

            var pos = _arenaView.transform.position + _offsetForArena;

            transform.DOMove(pos, 4).OnComplete(() =>
            {
                _arenaMenuView.Show();
            });
        }

        public void PlaySpeedParticle()
        {
            _particleSystem.gameObject.SetActive(true);
            _particleSystem.Play();
        }

        public void SetRefArenaView(ArenaView arenaView)
        {
            _arenaView = arenaView;
            _arenaView.PlayerTriggered += MoveCameraToArena;
        }
    }
}