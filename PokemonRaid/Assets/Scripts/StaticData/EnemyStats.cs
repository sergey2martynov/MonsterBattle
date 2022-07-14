using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "StaticData/EnemyStats", order = 53)]
    public class EnemyStats : ScriptableObject
    {
        [SerializeField] protected float _moveSpeed;
        [SerializeField] protected float _attackSpeed;
        [SerializeField] protected int _maxHealth;
        [SerializeField] protected int _health;
        [SerializeField] protected int _damage;
        [SerializeField] protected int _level;
        [SerializeField] protected int _maxTargetsAmount;
        [SerializeField] protected int _attackRange;
        
        public float MoveSpeed => _moveSpeed;
        public float AttackSpeed => _attackSpeed;
        public int MaxHealth => _maxHealth;
        public int Health => _health;
        public int Damage => _damage;
        public int Level => _level;
        public int MaxTargetsAmount => _maxTargetsAmount;
        public int AttackRange => _attackRange;
    }
}