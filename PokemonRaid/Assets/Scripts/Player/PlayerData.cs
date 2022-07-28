using System;
using System.Threading;
using Pokemon.PokemonHolder;
using StaticData;
using UnityEngine;

namespace Player
{
    public class PlayerData
    {
        private PokemonHolderModel _pokemonHolderModel;
        private float _moveSpeed;
        private int _maxHealth;
        private int _health;
        private int _level;
        private int _maxLevel = 100;
        private int _coins;
        public CancellationTokenSource Source { get; protected set; }
        
        public Vector3 MoveDirection { get; set; }
        public Vector3 LookDirection { get; set; }

        public float MoveSpeed
        {
            get => _moveSpeed;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Move speed cannot be equal or less than zero");
                }

                _moveSpeed = value;
            }
        }

        public int MaxHealth
        {
            get => _maxHealth;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Max health cannot be equal or less than zero");
                }
                
                _maxHealth = value;
            }
        }

        public int Health
        {
            get => _health;
            set
            {
                _health = value;

                if (_health <= 0)
                {
                    _health = 0;
                    
                    PlayerDied?.Invoke();
                }
                else
                {
                    _health = value;
                }
                
                HealthChange ?.Invoke(_health, _maxHealth);
            } 
        }

        public int Level
        {
            get => _level;
            set
            {
                if (value <= 0 || value > _maxLevel)
                {
                    throw new ArgumentException("Level cannot be equal or less than zero or more than " + _maxLevel);
                }
                
                if(value == 1)
                    FirstLevelFinished?.Invoke();

                _level = value;
            }
        }
        
        public int Coins
        {
            get => _coins;
            set
            {
                if (value < 0)
                {
                    _coins = 0;
                }

                _coins = value;
                CoinsAmountChanged?.Invoke(_coins);
            }
        }

        public event Action<int, int> HealthChange;
        public event Action<int> CoinsAmountChanged;
        public event Action FirstLevelFinished;
        public event Action PlayerDied;
        public event Func<Vector3> DirectionCorrectionRequested;

        public virtual void Initialize(PlayerStats stats, PokemonHolderModel pokemonHolderModel)
        {
            _pokemonHolderModel = pokemonHolderModel;
            SetStats(stats);
        }

        public void Initialize(PlayerStats stats, int level, int coinsAmount, PokemonHolderModel pokemonHolderModel)
        {
            _pokemonHolderModel = pokemonHolderModel;
            SetStats(stats);
            Level = level;
            Coins = coinsAmount;
        }

        public CancellationTokenSource CreateCancellationTokenSource()
        {
            return Source = new CancellationTokenSource();
        }

        public void DisposeSource()
        {
            Source?.Cancel();
            Source?.Dispose();
        }
        
        public Vector3 GetCorrectedDirection()
        {
            return DirectionCorrectionRequested?.Invoke() ?? new Vector3(10f, 10f, 10f);
        }

        public void SetMaxHealth()
        {
           MaxHealth = _pokemonHolderModel.GetAllHealth();
        }
        
        protected virtual void SetStats(PlayerStats stats)
        {
            MoveSpeed = stats.MoveSpeed;
            Level = stats.Level;
            Health = _pokemonHolderModel.GetAllHealth();
            _coins = stats.Coins;
        }
    }
}