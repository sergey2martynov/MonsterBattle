using System;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class EnemyStatsByType
    {
        [SerializeField] private BaseEnemyView _viewPrefab;
        [SerializeField] private List<EnemyStatsByLevel> _statsByLevel;

        public BaseEnemyView ViewPrefab => _viewPrefab;

        public EnemyStatsByLevel GetStats(int level)
        {
            if (level > _statsByLevel.Count)
            {
                throw new ArgumentException("Wrong level parameter" + level);
            }

            return _statsByLevel[level - 1];
        }
    }
}