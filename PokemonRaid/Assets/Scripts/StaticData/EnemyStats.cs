using System;
using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "StaticData/EnemyStats", order = 53)]
    public class EnemyStats : ScriptableObject
    {
        [SerializeField] private List<EnemyStatsByLevel> _stats;

        public EnemyStatsByLevel GetStats(int level)
        {
            if (level > _stats.Count)
            {
                throw new ArgumentException("Wrong level parameter" + level);
            }

            return _stats[level - 1];
        }
    }
}