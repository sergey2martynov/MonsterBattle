using System;
using System.Collections.Generic;
using Attributes;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class LevelData
    {
        [SerializeField] private int _level;
        [NamedProperty("Spawn positions")]
        [SerializeField] private List<SpawnPositions> _spawnPositions;
        [SerializeField] private int _totalCoinsReward;

        public List<SpawnPositions> SpawnPositions => _spawnPositions;
        public int TotalCoinsReward => _totalCoinsReward;
    }
}