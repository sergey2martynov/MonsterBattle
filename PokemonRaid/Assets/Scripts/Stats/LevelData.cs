using System;
using System.Collections.Generic;
using Attributes;
using Enemy;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class LevelData
    {
        [SerializeField] private int _level;
        [NamedProperty("Spawn positions")]
        [SerializeField] private List<SpawnPositions> _spawnPositions;

        public List<SpawnPositions> SpawnPositions => _spawnPositions;
    }
}