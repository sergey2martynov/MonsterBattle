﻿using System;
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
        
        
        

        protected virtual void SetStats(PlayerStats stats)
        {
            MoveSpeed = stats.MoveSpeed;
            MaxHealth = stats.MaxHealth;
            Level = stats.Level;
            Health = _pokemonHolderModel.GetAllHealth();
            _coins = stats.Coins;
            
        }
    }
}