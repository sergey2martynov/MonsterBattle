using System;
using System.Collections.Generic;
using System.Linq;
using Enemy;
using Stats;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "StaticData/EnemyStats", order = 53)]
    public class EnemyStats : ScriptableObject
    {
        [SerializeField] private List<EnemyStatsByType> _statsByType;

        public EnemyStatsByType GetTypeStats(BaseEnemyView enemyView)
        {
            foreach (var stats in _statsByType.Where(stats => stats.ViewPrefab.GetType() == enemyView.GetType()))
            {
                return stats;
            }
            
            throw new ArgumentException("There is no stats of type " + enemyView.GetType());
        }
    }
}