using System;
using System.Threading;
using StaticData;
using UnityEngine;

namespace Enemy
{
    public abstract class BaseEnemyData
    {
        protected EnemyStats _stats;
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
            set => _health = value < 0 ? 0 : value;
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

        public event Action<int, int> DamageTaken;

        public void Initialize(EnemyStats stats)
        {
            _stats = stats;
            SetStats(stats);
        }

        public CancellationTokenSource CreateCancellationTokenSource()
        {
            return Source = new CancellationTokenSource();
        }
        
        public void TakeDamage(int damage)
        {
            if (damage < 0)
            {
                return;
            }

            Health -= Damage;
            DamageTaken?.Invoke(_health, _maxHealth);
        }
        
        protected virtual void SetStats(EnemyStats stats)
        {
            _moveSpeed = stats.MoveSpeed;
            _attackSpeed = stats.AttackSpeed;
            _maxHealth = stats.MaxHealth;
            _health = _maxHealth;
            _damage = stats.Damage;
            _level = stats.Level;
            _maxTargetsAmount = stats.MaxTargetsAmount;
            _attackRange = stats.AttackRange;
        }

        public void DisposeSource()
        {
            Source?.Cancel();
            Source?.Dispose();
        }
    }
}