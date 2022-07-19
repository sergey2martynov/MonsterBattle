using System;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class PokemonStatsByLevel
    {
        public float _moveSpeed;
        public float _attackSpeed;
        public int _maxHealth;
        public int _damage;
        public int _level;
        public int _maxLevel;
        public int _maxTargetsAmount;
        public int _attackRange;

        public float MoveSpeed => _moveSpeed;
        public float AttackSpeed => _attackSpeed;
        public int MaxHealth => _maxHealth;
        public int Damage => _damage;
        public int Level => _level;
        public int MaxLevel => _maxLevel;
        public int MaxTargetsAmount => _maxTargetsAmount;
        public int AttackRange => _attackRange;
    }
}