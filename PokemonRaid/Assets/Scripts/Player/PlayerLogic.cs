using System;
using System.Threading.Tasks;
using Analitycs;
using Arena;
using CameraFollow;
using CardsCollection;
using DG.Tweening;
using Enemy.EnemyModel;
using Helpers;
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
        private CameraView _cameraView;
        private ArenaLogic _arenaLogic;
        private RaycastHit[] _hit = new RaycastHit[1];
        private Collider[] _boundsInRange = new Collider[2];
        private float _rayCastDistance = 1f;
        private float _smooth = 0.1f;
        private bool _isMovingToArena;
        private static readonly int Blend = Animator.StringToHash("Blend");
        private static readonly int ThrowBall = Animator.StringToHash("ThrowBall");
        private readonly int _move = Animator.StringToHash("Move");


        public event Action<int> CoinsAdded;
        public event Action<int> GemsAdded;
        public event Action LevelUpped;

        public virtual void Initialize(PlayerView playerView, PlayerData playerData,
            UpdateHandler updateHandler, PokemonHolderModel pokemonHolderModel, EnemyDataHolder enemyDataHolder,
            UpgradeLevels upgradeLevels, PokemonAvailabilityLogic pokemonAvailabilityLogic, ArenaLogic arenaLogic,
            CameraView cameraView)
        {
            _view = playerView;
            _data = playerData;
            _updateHandler = updateHandler;
            _pokemonHolderModel = pokemonHolderModel;
            _enemyDataHolder = enemyDataHolder;
            _upgradeLevels = upgradeLevels;
            _pokemonAvailabilityLogic = pokemonAvailabilityLogic;
            _arenaLogic = arenaLogic;
            _cameraView = cameraView;
            _updateHandler.UpdateTicked += Update;
            //_enemyDataHolder.EnemyDefeated += OnEnemyDefeated;
            _view.ViewDestroyed += Dispose;
            _view.LevelFinished += IncreaseLevel;
            _data.DirectionCorrectionRequested += CheckForBounds;
            _data.HealthChange += OnHealthChange;
            _data.CoinsAmountChanged += OnCoinsAmountChanged;
            _data.GemsAmountChanged += OnGemsAmountChanged;
            _data.MoveAnimationRequested += ActivateMoveAnimation;
            _enemyDataHolder.EnemyDefeated += OnEnemyDefeated;
            _data.PositionSet += GoToArena;
            _enemyDataHolder.AllEnemiesDefeated += ActivateSpeed;
            _arenaLogic.ArenaCompleted += IncreaseLevel;
        }

        private void Update()
        {
            _view.Transform.position += _data.MoveDirection * _data.MoveSpeed * Time.deltaTime;

            if (!_isMovingToArena)
            {
                _view.Animator.SetFloat(Blend, _data.MoveDirection.magnitude, _smooth, Time.deltaTime);
            }
            else
            {
                _view.Animator.SetFloat(Blend, 0.8f, _smooth, Time.deltaTime);
            }

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
            return CollisionHandler.CheckForBounds(_view.Transform, _rayCastDistance, _boundsInRange, _hit,
                _data.LookDirection);
            // var boundsAmount = Physics.OverlapSphereNonAlloc(_view.Transform.position, _rayCastDistance, _boundsInRange,
            //     _view.BoundsLayer);
            //
            // if (boundsAmount == 0)
            // {
            //     return new Vector3(10f, 10f, 10f);
            // }
            //
            // var direction = (Vector3) _data.LookDirection;
            // var outDirection = (Vector3) _data.LookDirection;
            // direction.Normalize();
            //
            // foreach (var boundCollider in _boundsInRange)
            // {
            //     if (boundCollider == null)
            //     {
            //         continue;
            //     }
            //
            //     var position = _view.Transform.position;
            //     var positionDelta = boundCollider.transform.position - position;
            //     var ray = new Ray(position, positionDelta.normalized);
            //
            //     if (Physics.RaycastNonAlloc(ray, _hit, positionDelta.magnitude, _view.BoundsLayer) > 0)
            //     {
            //         var normal = new Vector3(
            //             Mathf.Clamp(_hit[0].normal.x, -Mathf.Abs(direction.x), Mathf.Abs(direction.x)),
            //             Mathf.Clamp(_hit[0].normal.y, -Mathf.Abs(direction.y), Mathf.Abs(direction.y)),
            //             Mathf.Clamp(_hit[0].normal.z, -Mathf.Abs(direction.z), Mathf.Abs(direction.z)));
            //         
            //         var xSign = direction.x == 0 ? 0f : Mathf.Sign(direction.x); 
            //         var ySign = direction.y == 0 ? 0f : Mathf.Sign(direction.y); 
            //         var zSign = direction.z == 0 ? 0f : Mathf.Sign(direction.z);
            //         
            //         if (Vector3.Angle(normal, direction) <= 90)
            //         {
            //             continue;
            //         }
            //         
            //         outDirection -= new Vector3(Mathf.Abs(normal.x) * xSign, Mathf.Abs(normal.y) * ySign,
            //             Mathf.Abs(normal.z) * zSign);
            //     }
            // }
            //
            // Array.Clear(_boundsInRange, 0, _boundsInRange.Length);
            // Array.Clear(_hit, 0, _hit.Length);
            // return outDirection;
        }

        private async void ActivateMoveAnimation(float duration)
        {
            var delay = (int)duration * 1000;
            _isMovingToArena = true;
            await Task.Delay(delay);
            _isMovingToArena = false;
        }

        public void HealthBarDisabler()
        {
            _view.HealthBarView.gameObject.SetActive(true);
        }

        private void IncreaseLevel()
        {
            _data.Level++;

            if (_data.Level == _upgradeLevels.ListUpgradeLevels[0] ||
                _data.Level == _upgradeLevels.ListUpgradeLevels[2])
            {
                _pokemonAvailabilityLogic.UnLockNewTypeRangePokemon();
            }
            else if (_data.Level == _upgradeLevels.ListUpgradeLevels[1])
            {
                _pokemonAvailabilityLogic.UnLockNewTypeMeleePokemon();
            }

            EventSender.SendLevelFinish();

            _data.IncreaseLevelCount();

            LevelUpped?.Invoke();
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

        public void OnPurchase()
        {
            _view.Animator.SetTrigger(ThrowBall);
        }

        private void GoToArena(Vector3 newPosition)
        {
            _view.transform.DOMove(newPosition, 3);
            ActivateMoveAnimation(3);
        }

        private void OnCoinsAmountChanged(int coins)
        {
            CoinsAdded?.Invoke(coins);
        }
        
        private void OnGemsAmountChanged(int gems)
        {
            GemsAdded?.Invoke(gems);
        }

        private void Dispose()
        {
            _data.HealthChange -= OnHealthChange;
            _updateHandler.UpdateTicked -= Update;
            //_enemyDataHolder.EnemyDefeated -= OnEnemyDefeated;
            _data.DisposeSource();
            _view.ViewDestroyed -= Dispose;
            _view.LevelFinished -= IncreaseLevel;
            _data.DirectionCorrectionRequested -= CheckForBounds;
            _data.CoinsAmountChanged -= OnCoinsAmountChanged;
            _enemyDataHolder.EnemyDefeated -= OnEnemyDefeated;
            _data.PositionSet -= GoToArena;
            _data.MoveAnimationRequested -= ActivateMoveAnimation;
            _arenaLogic.ArenaCompleted -= IncreaseLevel;
            _enemyDataHolder.AllEnemiesDefeated -= ActivateSpeed;
        }

        private void ActivateSpeed()
        {
            _cameraView.PlaySpeedParticle();
        }
    }
}