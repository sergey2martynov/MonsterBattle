using System;
using System.Data;
using System.Threading;
using StaticData;
using Stats;
using UnityEngine;

namespace Pokemon
{
    public abstract class PokemonDataBase
    {
        private PokemonStatsByLevel _pokemonStats;
        private float _moveSpeed;
        private float _attackSpeed;
        private float _attackTime;
        private int _maxHealth;
        private int _health;
        private int _damage;
        private int _level;
        private int _maxLevel;
        private int _maxTargetsAmount;
        private int _attackRange;

        public CancellationTokenSource Source { get; protected set; }

        public Vector3 MoveDirection { get; set; }
        public int[] Indexes { get; set; }

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

        public float AttackSpeed
        {
            get => _attackSpeed;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Attack speed cannot be equal or less than zero");
                }

                _attackSpeed = value;
            }
        }

        public float AttackTime
        {
            get => _attackTime;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Attack time cannot be less than zero");
                }

                _attackTime = value;
            }
        }

        public int MaxHealth
        {
            get => _maxHealth;
            set
            {
                if (value <= 0)
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
                if (value < 0)
                {
                    _health = 0;
                    PokemonDied?.Invoke();
                }
                else
                {
                    _health = value;
                }
                
                HealthChanged?.Invoke(_health, _maxHealth);
            }
        }

        public int Damage
        {
            get => _damage;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Damage cannot be less than zero");
                }

                _damage = value;
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

                _level = value;
            }
        }

        public int MaxLevel
        {
            get => _maxLevel;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Max level cannot be equal or less than zero");
                }

                _maxLevel = value;
            }
        }

        public int MaxTargetsAmount
        {
            get => _maxTargetsAmount;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Max targets amount cannot be equal or less than zero");
                }

                _maxTargetsAmount = value;
            }
        }

        public int AttackRange
        {
            get => _attackRange;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Attack range cannot be equal or less than zero");
                }

                _attackRange = value;
            }
        }
        
        public event Action<int, int> HealthChanged; 
        public event Action PokemonDied;

        public virtual void Initialize(PokemonStatsByLevel stats, int[] indexes)
        {
            Indexes = indexes;
            SetStats(stats);
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

        protected virtual void SetStats(PokemonStatsByLevel stats)
        {
            MoveSpeed = stats.MoveSpeed;
            AttackSpeed = stats.AttackSpeed;
            MaxHealth = stats.MaxHealth;
            Health = _maxHealth;
            Damage = stats.Damage;
            MaxLevel = stats.MaxLevel;
            Level = stats.Level;
            MaxTargetsAmount = stats.MaxTargetsAmount;
            AttackRange = stats.AttackRange;
        }
    }
}