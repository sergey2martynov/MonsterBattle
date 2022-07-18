using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "LevelDataHolder", menuName = "StaticData/LevelDataHolder", order = 54)]
    public class LevelDataHolder : ScriptableObject
    {
        [SerializeField] private List<LevelData> _levelData;

        public LevelData GetLevelData(int level)
        {
            return _levelData[level - 1];
        }
    }
}