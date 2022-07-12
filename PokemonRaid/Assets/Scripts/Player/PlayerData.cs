using System;
using System.Threading;
using StaticData;

namespace Player
{
    public class PlayerData
    {
        private PokemonPrefabHolder _pokemonPrefabHolder;
        private int _maxHealth;
        private int _health;
        private int _level;
        private int _maxLevel;
        private int _coins;
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
        
        public int Coins
        {
            get => _coins;
            set => _coins = value < 0 ? 0 : value;
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