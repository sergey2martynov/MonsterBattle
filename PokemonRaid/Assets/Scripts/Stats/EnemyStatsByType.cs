using System;
using System.Collections.Generic;
using Attributes;
using Enemy;

namespace Stats
{
    [Serializable]
    public class EnemyStatsByType
    {
        public BaseEnemyView _viewPrefab;
        [NamedList(new []{"Level 1", "Level 2", "Level 3", "Level 4", "Level 5"})]
        public List<EnemyStatsByLevel> _statsByLevel;

        public BaseEnemyView ViewPrefab => _viewPrefab;

        public EnemyStatsByLevel GetStats(int level)
        {
            if (level > _statsByLevel.Count)
            {
                throw new ArgumentException("Wrong level parameter" + level);
            }

            return _statsByLevel[level - 1];
        }

        public void AddNewItem()
        {
            _statsByLevel.Add(new EnemyStatsByLevel());
        }

        public void RemoveItem(int index)
        {
            _statsByLevel.RemoveAt(index);
        }
    }
}