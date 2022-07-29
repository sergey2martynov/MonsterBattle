using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "BossStatsByLevel", menuName = "StaticData/BossStatsByLevel", order = 70)]
    public class BossStatsByLevel : EnemyStats
    {
        [SerializeField] private List<BossStats> _bossStats;

        public BossStats GetBossStats(int level)
        {
            return _bossStats[level];
        }
    }
}