using System;
using System.Collections.Generic;
using Attributes;
using Enemy;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class SpawnPositions
    {
        [NamedProperty("Prefab")]
        [SerializeField] private List<BaseEnemyView> _enemyPrefabs;
        [NamedProperty("Position")]
        [SerializeField] private List<Vector3> _positions;

        public List<Vector3> Positions => _positions;
        public List<BaseEnemyView> EnemyPrefabs => _enemyPrefabs;
    }
}