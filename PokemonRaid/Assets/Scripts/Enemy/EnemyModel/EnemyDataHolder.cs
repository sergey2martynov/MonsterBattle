﻿using System;
using System.Collections.Generic;

namespace Enemy.EnemyModel
{
    public class EnemyDataHolder
    {
        private readonly List<BaseEnemyData> _enemiesData = new List<BaseEnemyData>();
        private float _coinsRewardPerEnemy;
        private int _countKilledEnemy;

        public int CountKilledEnemy => _countKilledEnemy;

        public float CoinsRewardPerEnemy
        {
            get => _coinsRewardPerEnemy;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Reward cannot be equal or less than zero");
                }

                _coinsRewardPerEnemy = value;
            }
        }

        public event Action<int> EnemyDefeated;
        
        public void AddEnemyData(BaseEnemyData data)
        {
            if (_enemiesData.Contains(data))
            {
                return;
            }
            
            _enemiesData.Add(data);
            data.EnemyDied += RemoveEnemyData;
        }

        private void RemoveEnemyData(BaseEnemyData data)
        {
            _enemiesData.Remove(data);
            _countKilledEnemy++;
            data.EnemyDied -= RemoveEnemyData;
            OnEnemyDied();
        }

        private void OnEnemyDied()
        {
            //EnemyDefeated?.Invoke(_coinsRewardPerEnemy);
        }
    }
}