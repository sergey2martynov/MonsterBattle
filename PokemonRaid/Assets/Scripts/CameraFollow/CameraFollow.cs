using System;
using DG.Tweening;
using Player;
using Shop;
using UnityEngine;

namespace CameraFollow
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private ShopView _shopView;

        private bool _isCanAddOffset = true;
        private Vector3 _offset = new Vector3(0, 18, -6.68f);

        private void Start()
        {
            _shopView.StartButtonPressed += MoveCamera;
        }

        private void OnDestroy()
        {
            _shopView.StartButtonPressed -= MoveCamera;
        }


        private void LateUpdate()
        {
            if (_isCanAddOffset)
                transform.position = _playerView.transform.position + _offset;
        }

        private void MoveCamera()
        {
            _isCanAddOffset = false;

            transform.DOMove(new Vector3(0, 15, 4.18f), 2).OnComplete(() =>
            {
                _offset = new Vector3(0, 15, -2.5f);

                _isCanAddOffset = true;
            });
        }
    }
}