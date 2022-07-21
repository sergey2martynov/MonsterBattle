using System;
using SerializedObjects;
using Stats;
using UnityEngine;

namespace Pokemon
{
    [Serializable]
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
        //private SerializableVector3 _moveDirection;

        // public CancellationTokenSource Source { get; protected set; }

        public SerializableVector3 MoveDirection { get; set; }
        public SerializableVector3 LookDirection { get; set; }

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
                if (value <= 0)
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
        
        [field: NonSerialized]
        public event Action<int, int> HealthChanged; 
        [field: NonSerialized]
        public event Action PokemonDied;
        [field: NonSerialized] 
        public event Func<Vector3> DirectionCorrectionRequested; 

        public virtual void Initialize(PokemonStatsByLevel stats, int[] indexes)
        {
            Indexes = indexes;
            SetStats(stats);
        }

        public virtual void Initialize()
        {
            Health = MaxHealth;
            _attackTime = 0f;
        }

        // public CancellationTokenSource CreateCancellationTokenSource()
        // {
        //     return Source = new CancellationTokenSource();
        // }

        // public void DisposeSource()
        // {
        //     Source?.Cancel();
        //     Source?.Dispose();
        // }

        public Vector3 GetCorrectedDirection()
        {
            return DirectionCorrectionRequested?.Invoke() ?? new Vector3(10f, 10f, 10f);
        }

        protected virtual void SetStats(PokemonStatsByLevel stats)
        {
            AttackTime = 0;
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