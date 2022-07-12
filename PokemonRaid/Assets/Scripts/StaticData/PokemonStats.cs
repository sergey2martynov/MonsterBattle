using System.Collections.Generic;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "PokemonStats", menuName = "StaticData/PokemonStats", order = 52)]
    public class PokemonStats : ScriptableObject
    {
        [SerializeField] protected float _moveSpeed;
        [SerializeField] protected float _attackSpeed;
        [SerializeField] protected int _maxHealth;
        [SerializeField] protected int _health;
        [SerializeField] protected int _damage;
        [SerializeField] protected int _level;
        [SerializeField] protected int _maxLevel;
        [SerializeField] protected int _maxTargetsAmount;
        [SerializeField] protected int _attackRange;
        
        public float MoveSpeed => _moveSpeed;
        public float AttackSpeed => _attackSpeed;
        public int MaxHealth => _maxHealth;
        public int Health => _health;
        public int Damage => _damage;
        public int Level => _level;
        public int MaxLevel => _maxLevel;
        public int MaxTargetsAmount => _maxTargetsAmount;
        public int AttackRange => _attackRange;
    }
}