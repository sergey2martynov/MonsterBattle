using System;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

namespace StaticData
{
    [Serializable]
    public class PokemonListForArena
    {
        [SerializeField] private int _level;
        [SerializeField] private List<int> _levelsEnemy;
        [SerializeField] private List<BaseEnemyView> _enemies;

        public int Level => _level;
        public List<int> LevelsEnemy => _levelsEnemy;
        public List<BaseEnemyView> Enemies => _enemies;
    }
}