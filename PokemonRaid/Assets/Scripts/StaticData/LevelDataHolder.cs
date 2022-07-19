using System.Collections.Generic;
using Attributes;
using Stats;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "LevelDataHolder", menuName = "StaticData/LevelDataHolder", order = 54)]
    public class LevelDataHolder : ScriptableObject
    {
        //[NamedList(new []{"Level 1", "Level 2", "Level 3"})]
        [NamedProperty("Level")]
        [SerializeField] private List<LevelData> _levelData;

        public LevelData GetLevelData(int level)
        {
            return _levelData[level - 1];
        }
    }
}