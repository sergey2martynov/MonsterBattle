using System;
using Enemy.EnemyModel;
using Pokemon.PokemonHolder;
using UnityEngine;
using UpdateHandlerFolder;

namespace Player
{
    public class PlayerLogic
    {
        private PlayerView _view;
        private PlayerData _data;
        private UpdateHandler _updateHandler;
        private EnemyDataHolder _enemyDataHolder;
        private PokemonHolderModel _pokemonHolderModel;
        private RaycastHit[] _hit = new RaycastHit[1];
        private float _rayCastDistance = 1.5f;

        public event Action<int> CoinsAdded; 

        public virtual void Initialize( PlayerView playerView, PlayerData playerData,
            UpdateHandler updateHandler, PokemonHolderModel pokemonHolderModel, EnemyDataHolder enemyDataHolder)
        {
            _view = playerView;
            _data = playerData;
            _updateHandler = updateHandler;
            _pokemonHolderModel = pokemonHolderModel;
            _enemyDataHolder = enemyDataHolder;
            _updateHandler.UpdateTicked += Update;
            _enemyDataHolder.EnemyDefeated += OnEnemyDefeated;
            _view.ViewDestroyed += Dispose;
            _data.DirectionCorrectionRequested += CheckForBounds;
            _data.HealthChange += OnHealthChange;
            _data.CoinsAmountChanged += OnCoinsAmountChanged;
        }

        private void Update()
        {
            _view.Transform.position += _data.MoveDirection * _data.MoveSpeed * Time.deltaTime;

            if (_data.LookDirection.magnitude != 0)
            {
                RotateAt(_data.LookDirection);
            }
        }
        
        public void RotateAt(Vector3 point)
        {
            var destinationRotation = Quaternion.LookRotation(point, Vector3.up);
            _view.Transform.rotation =
                Quaternion.RotateTowards(_view.Transform.rotation, destinationRotation, 720 * Time.deltaTime);
        }
        
        public Vector3 CheckForBounds()
        {
            var ray = new Ray(_view.Transform.position, _view.Transform.forward);

            if (Physics.RaycastNonAlloc(ray, _hit, _rayCastDistance, _view.BoundsLayer) > 0)
            {
                var direction = _data.LookDirection;
                direction.Normalize();
                var normal = _hit[0].normal;
                
                if (Vector3.SignedAngle(normal, _hit[0].point, Vector3.up) < 0)
                {
                    return direction - new Vector3(direction.x * normal.x, direction.y * normal.y, direction.z * normal.z);
                }
                else
                {
                    return direction + new Vector3(direction.x * normal.x, direction.y * normal.y, direction.z * normal.z);
                }
            }

            return new Vector3(10f, 10f, 10f);
        }

        public void HealthBarDisabler()
        {
            _view.HealthBarView.gameObject.SetActive(true);
        }

        private void OnHealthChange(int health, int maxHealth)
        {
            _view.SetHealth(_data.Health / (float)_data.MaxHealth);
        }

        private void OnEnemyDefeated(int coinsReward)
        {
            if (coinsReward <= 0)
            {
                return;
            }
            
            _data.Coins += coinsReward;
        }

        private void OnCoinsAmountChanged(int coins)
        {
            CoinsAdded?.Invoke(coins);
        }

        private void Dispose()
        {
            _data.HealthChange -= OnHealthChange;
            _updateHandler.UpdateTicked -= Update;
            _enemyDataHolder.EnemyDefeated -= OnEnemyDefeated;
            _data.DisposeSource();
            _view.ViewDestroyed -= Dispose;
            _data.DirectionCorrectionRequested -= CheckForBounds;
            _data.CoinsAmountChanged -= OnCoinsAmountChanged;

        }
    }
}