﻿using System;
using Analitycs;
using CardsCollection;
using DG.Tweening;
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
        private Collider[] _boundsInRange = new Collider[2];
        private float _rayCastDistance = 1f;
        private float _smooth = 0.1f;
        private static readonly int Blend = Animator.StringToHash("Blend");
        private static readonly int ThrowBall = Animator.StringToHash("ThrowBall");

        public event Action<int> CoinsAdded;
        public event Action LevelUpped;

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
            //_enemyDataHolder.EnemyDefeated += OnEnemyDefeated;
            _view.ViewDestroyed += Dispose;
            _view.LevelFinished += IncreaseLevel;
            _data.DirectionCorrectionRequested += CheckForBounds;
            _data.HealthChange += OnHealthChange;
            _data.CoinsAmountChanged += OnCoinsAmountChanged;
            _enemyDataHolder.EnemyDefeated += OnEnemyDefeated;
            _data.PositionSeted += GoToArena;
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

        // public Vector3 CheckForBounds()
        // {
        //     var ray = new Ray(_view.Transform.position, _view.Transform.forward);
        //
        //     if (Physics.RaycastNonAlloc(ray, _hit, _rayCastDistance, _view.BoundsLayer) > 0)
        //     {
        //         var direction = _data.LookDirection;
        //         direction.Normalize();
        //         //var normal = _hit[0].normal;
        //         var normal = new Vector3(
        //             Mathf.Clamp(_hit[0].normal.x, -Mathf.Abs(direction.x), Mathf.Abs(direction.x)),
        //             Mathf.Clamp(_hit[0].normal.y, -Mathf.Abs(direction.y), Mathf.Abs(direction.y)),
        //             Mathf.Clamp(_hit[0].normal.z, -Mathf.Abs(direction.z), Mathf.Abs(direction.z)));
        //
        //         // return direction - new Vector3(Mathf.Abs(normal.x) * direction.x, Mathf.Abs(normal.y) * direction.y,
        //         //     Mathf.Abs(normal.z) * direction.z);
        //
        //         var xSign = direction.x == 0 ? 0f : Mathf.Sign(direction.x);
        //         var ySign = direction.y == 0 ? 0f : Mathf.Sign(direction.y);
        //         var zSign = direction.z == 0 ? 0f : Mathf.Sign(direction.z);
        //
        //         return direction - new Vector3(Mathf.Abs(normal.x) * xSign, Mathf.Abs(normal.y) * ySign,
        //             Mathf.Abs(normal.z) * zSign);
        //
        //         // return direction - new Vector3(
        //         //     Mathf.Abs(normal.x) * direction.x != 0 ? Mathf.Sign(direction.x) : 0f,
        //         //     Mathf.Abs(normal.y) * direction.y != 0 ? Mathf.Sign(direction.y) : 0f,
        //         //     Mathf.Abs(normal.z) * direction.z != 0 ? Mathf.Sign(direction.z) : 0f);
        //     }
        //
        //     return new Vector3(10f, 10f, 10f);
        // }
        
        public Vector3 CheckForBounds()
        {
            var boundsAmount = Physics.OverlapSphereNonAlloc(_view.Transform.position, _rayCastDistance, _boundsInRange,
                _view.BoundsLayer);

            if (boundsAmount == 0)
            {
                return new Vector3(10f, 10f, 10f);
            }

            var direction = (Vector3) _data.LookDirection;
            var outDirection = (Vector3) _data.LookDirection;
            direction.Normalize();
            
            foreach (var boundCollider in _boundsInRange)
            {
                if (boundCollider == null)
                {
                    continue;
                }

                var position = _view.Transform.position;
                var positionDelta = boundCollider.transform.position - position;
                var ray = new Ray(position, positionDelta.normalized);

                if (Physics.RaycastNonAlloc(ray, _hit, positionDelta.magnitude, _view.BoundsLayer) > 0)
                {
                    var normal = new Vector3(
                        Mathf.Clamp(_hit[0].normal.x, -Mathf.Abs(direction.x), Mathf.Abs(direction.x)),
                        Mathf.Clamp(_hit[0].normal.y, -Mathf.Abs(direction.y), Mathf.Abs(direction.y)),
                        Mathf.Clamp(_hit[0].normal.z, -Mathf.Abs(direction.z), Mathf.Abs(direction.z)));
                    
                    var xSign = direction.x == 0 ? 0f : Mathf.Sign(direction.x); 
                    var ySign = direction.y == 0 ? 0f : Mathf.Sign(direction.y); 
                    var zSign = direction.z == 0 ? 0f : Mathf.Sign(direction.z);
                    
                    if (Vector3.Angle(normal, direction) <= 90)
                    {
                        continue;
                    }
                    
                    outDirection -= new Vector3(Mathf.Abs(normal.x) * xSign, Mathf.Abs(normal.y) * ySign,
                        Mathf.Abs(normal.z) * zSign);
                }
            }
            
            Array.Clear(_boundsInRange, 0, _boundsInRange.Length);
            Array.Clear(_hit, 0, _hit.Length);
            return outDirection;
        }
        
        // public Vector3 CheckForBounds()
        // {
        //     var ray = new Ray(_view.Transform.position, _data.LookDirection);
        //
        //     if (Physics.SphereCastNonAlloc(ray, _rayCastDistance, _hit, 0.1f, _view.BoundsLayer) > 0)
        //     {
        //         var direction = _data.LookDirection;
        //         var outDirection = _data.LookDirection;
        //         direction.Normalize();
        //
        //         foreach (var hit in _hit)
        //         {
        //             var normal = new Vector3(
        //                 Mathf.Clamp(_hit[0].normal.x, -Mathf.Abs(direction.x), Mathf.Abs(direction.x)),
        //                 Mathf.Clamp(_hit[0].normal.y, -Mathf.Abs(direction.y), Mathf.Abs(direction.y)),
        //                 Mathf.Clamp(_hit[0].normal.z, -Mathf.Abs(direction.z), Mathf.Abs(direction.z)));
        //             
        //             var xSign = direction.x == 0 ? 0f : Mathf.Sign(direction.x); 
        //             var ySign = direction.y == 0 ? 0f : Mathf.Sign(direction.y); 
        //             var zSign = direction.z == 0 ? 0f : Mathf.Sign(direction.z); 
        //             outDirection -= new Vector3(Mathf.Abs(normal.x) * xSign, Mathf.Abs(normal.y) * ySign,
        //                 Mathf.Abs(normal.z) * zSign);
        //         }
        //         
        //         Array.Clear(_hit, 0, _hit.Length);
        //         return outDirection;
        //     }
        //
        //     Array.Clear(_hit, 0, _hit.Length);
        //     return new Vector3(10f, 10f, 10f);
        // }

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

        public void OnPurchase()
        {
            _view.Animator.SetTrigger(ThrowBall);
        }

        private void GoToArena(Vector3 newPosition)
        {
            _view.transform.DOMove(newPosition, 3);
        }


            private void OnCoinsAmountChanged(int coins)
        {
            CoinsAdded?.Invoke(coins);
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
            _data.PositionSeted -= GoToArena;
        }
    }
}