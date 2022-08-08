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
        [SerializeField] private List<SpawnPositions> _spawnPositions;
        [SerializeField] private int _totalCoinsReward;
        [SerializeField] private int _totalGemsReward;
        [SerializeField] private GameObject _environment;

        public List<SpawnPositions> SpawnPositions => _spawnPositions;
        public int TotalCoinsReward => _totalCoinsReward;
        public int TotalGemsReward => _totalGemsReward;
        public GameObject Environment => _environment;
    }
}