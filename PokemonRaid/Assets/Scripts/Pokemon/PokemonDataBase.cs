using System;
using System.Threading;
using Pokemon.States;
using UnityEngine;

namespace Pokemon
{
    public abstract class PokemonDataBase
    {
        protected ScriptableObject PokemonData;
        protected int _maxHealth;
        protected int _health;
        protected int _damage;
        protected int _level;
        protected int _maxLevel;

        public BaseState CurrentState { get; set; }

        public CancellationTokenSource Source { get; protected set; }

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
                if (value <= 0 || value > _maxLevel)
                {
                    throw new ArgumentException("Level cannot be equal or less than zero or more than " + _maxLevel);
                }

                _level = value;
            }
        }

        public virtual void Initialize()
        {
            SetStats();
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

        protected virtual void SetStats()
        {
            
        }
    }
}