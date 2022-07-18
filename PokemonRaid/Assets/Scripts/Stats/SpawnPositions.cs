using System;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class SpawnPositions
    {
        [SerializeField] private List<BaseEnemyView> _enemyPrefabs;
        [SerializeField] private List<Vector3> _positions;
        
        public List<Vector3> Positions => _positions;
        public List<BaseEnemyView> EnemyPrefabs => _enemyPrefabs;
    }
}