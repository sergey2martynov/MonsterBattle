using System.Collections.Generic;
using Arena;
using Attributes;
using Stats;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "LevelDataHolder", menuName = "StaticData/LevelDataHolder", order = 54)]
    public class LevelDataHolder : ScriptableObject
    {
        //[NamedList(new []{"Level 1", "Level 2", "Level 3"})]
        [NamedProperty("Level")] [SerializeField]
        private List<LevelData> _levelData;

        [SerializeField] private ArenaView _arenaView;

        public ArenaView ArenaView => _arenaView;

        public LevelData GetLevelData(int level)
        {
            if (level <= _levelData.Count)
            {
                return _levelData[level - 1];
            }

            return null;
        }
    }
}