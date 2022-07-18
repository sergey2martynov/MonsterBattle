using System;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class LevelData
    {
        [SerializeField] private int _level;
        [SerializeField] private List<SpawnPositions> _spawnPositions;

        public List<SpawnPositions> SpawnPositions => _spawnPositions;
    }
}