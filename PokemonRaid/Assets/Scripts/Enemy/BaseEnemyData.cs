using System;
using System.Threading;
using Stats;
using UnityEngine;

namespace Enemy
{
    public abstract class BaseEnemyData
    {
        protected EnemyStatsByLevel _stats;
        protected float _moveSpeed;
        protected float _attackSpeed;
        protected int _maxHealth;
        protected int _health;
        protected int _damage;
        protected int _level;
        protected int _maxTargetsAmount;
        protected int _attackRange;

        public CancellationTokenSource Source { get; protected set; }
        public Vector3 MoveDirection { get; set; }
        public float AttackTime { get; set; }
        public Vector3 InitialPosition { get; set; }

        public float MoveSpeed
        {
            get => _moveSpeed;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Move speed cannot be less than zero");
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
                    EnemyDied?.Invoke(this);
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
                if (value <= 0)
                {
                    throw new ArgumentException("Level cannot be equal or less than zero");
                }

                _level = value;
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
        public event Action<BaseEnemyData> EnemyDied;
        public event Action<bool> IdleStateRequested;

        public virtual void Initialize(EnemyStatsByLevel stats)
        {
            _stats = stats;
            SetStats(stats);
        }

        public CancellationTokenSource CreateCancellationTokenSource()
        {
            return Source = new CancellationTokenSource();
        }
        
        protected virtual void SetStats(EnemyStatsByLevel stats)
        {
            AttackTime = 0;
            MoveSpeed = stats.MoveSpeed;
            AttackSpeed = stats.AttackSpeed;
            MaxHealth = stats.MaxHealth;
            Health = _maxHealth;
            Damage = stats.Damage;
            Level = stats.Level;
            MaxTargetsAmount = stats.MaxTargetsAmount;
            AttackRange = stats.AttackRange;
        }

        public void OnIdleStateRequired(bool isRequired)
        {
            IdleStateRequested?.Invoke(isRequired);
        }

        public void DisposeSource()
        {
            Source?.Cancel();
            Source?.Dispose();
            Source = null;
        }
    }
}