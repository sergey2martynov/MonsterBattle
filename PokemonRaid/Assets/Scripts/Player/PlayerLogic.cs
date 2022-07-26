using System;
using CardsCollection;
using Enemy.EnemyModel;
using Pokemon.PokemonHolder;
using StaticData;
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
        private UpgradeLevels _upgradeLevels;
        private PokemonAvailabilityLogic _pokemonAvailabilityLogic;
        private PokemonHolderModel _pokemonHolderModel;
        private RaycastHit[] _hit = new RaycastHit[1];
        private float _rayCastDistance = 1.5f;
        private float _smooth = 0.1f;
        private static readonly int Blend = Animator.StringToHash("Blend");

        public event Action<int> CoinsAdded;

        public virtual void Initialize(PlayerView playerView, PlayerData playerData,
            UpdateHandler updateHandler, PokemonHolderModel pokemonHolderModel, EnemyDataHolder enemyDataHolder,
            UpgradeLevels upgradeLevels, PokemonAvailabilityLogic pokemonAvailabilityLogic)
        {
            _view = playerView;
            _data = playerData;
            _updateHandler = updateHandler;
            _pokemonHolderModel = pokemonHolderModel;
            _enemyDataHolder = enemyDataHolder;
            _upgradeLevels = upgradeLevels;
            _pokemonAvailabilityLogic = pokemonAvailabilityLogic;
            _updateHandler.UpdateTicked += Update;
            _enemyDataHolder.EnemyDefeated += OnEnemyDefeated;
            _view.ViewDestroyed += Dispose;
            _view.LevelFinished += IncreaseLevel;
            _data.DirectionCorrectionRequested += CheckForBounds;
            _data.HealthChange += OnHealthChange;
            _data.CoinsAmountChanged += OnCoinsAmountChanged;
        }

        private void Update()
        {
            _view.Transform.position += _data.MoveDirection * _data.MoveSpeed * Time.deltaTime;
            _view.Animator.SetFloat(Blend, _data.MoveDirection.magnitude, _smooth, Time.deltaTime);

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
                //var normal = _hit[0].normal;
                var normal = new Vector3(
                    Mathf.Clamp(_hit[0].normal.x, -Mathf.Abs(direction.x), Mathf.Abs(direction.x)),
                    Mathf.Clamp(_hit[0].normal.y, -Mathf.Abs(direction.y), Mathf.Abs(direction.y)),
                    Mathf.Clamp(_hit[0].normal.z, -Mathf.Abs(direction.z), Mathf.Abs(direction.z)));

                // return direction - new Vector3(Mathf.Abs(normal.x) * direction.x, Mathf.Abs(normal.y) * direction.y,
                //     Mathf.Abs(normal.z) * direction.z);

                var xSign = direction.x == 0 ? 0f : Mathf.Sign(direction.x);
                var ySign = direction.y == 0 ? 0f : Mathf.Sign(direction.y);
                var zSign = direction.z == 0 ? 0f : Mathf.Sign(direction.z);

                return direction - new Vector3(Mathf.Abs(normal.x) * xSign, Mathf.Abs(normal.y) * ySign,
                    Mathf.Abs(normal.z) * zSign);

                // return direction - new Vector3(
                //     Mathf.Abs(normal.x) * direction.x != 0 ? Mathf.Sign(direction.x) : 0f,
                //     Mathf.Abs(normal.y) * direction.y != 0 ? Mathf.Sign(direction.y) : 0f,
                //     Mathf.Abs(normal.z) * direction.z != 0 ? Mathf.Sign(direction.z) : 0f);
            }

            return new Vector3(10f, 10f, 10f);
        }

        public void HealthBarDisabler()
        {
            _view.HealthBarView.gameObject.SetActive(true);
        }

        private void IncreaseLevel()
        {
            _data.Level++;

            if (_data.Level == _upgradeLevels.ListUpgradeLevels[0] ||
                _data.Level == _upgradeLevels.ListUpgradeLevels[2] ||
                _data.Level == _upgradeLevels.ListUpgradeLevels[4])
            {
                _pokemonAvailabilityLogic.UnLockNewTypeRangePokemon();
            }
            else if (_data.Level == _upgradeLevels.ListUpgradeLevels[1] ||
                     _data.Level == _upgradeLevels.ListUpgradeLevels[3])
            {
                _pokemonAvailabilityLogic.UnLockNewTypeMeleePokemon();
            }
        }

        private void OnHealthChange(int health, int maxHealth)
        {
            _view.SetHealth(_data.Health / (float) _data.MaxHealth);
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
            _view.LevelFinished += IncreaseLevel;
            _data.DirectionCorrectionRequested -= CheckForBounds;
            _data.CoinsAmountChanged -= OnCoinsAmountChanged;
        }
    }
}