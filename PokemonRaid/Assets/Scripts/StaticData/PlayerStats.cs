using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "StaticData/PlayerStats", order = 53)]
    public class PlayerStats : ScriptableObject
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _health;
        [SerializeField] private int _level;
        [SerializeField] private int _coins;

        public float MoveSpeed => _moveSpeed;
        public int MaxHealth => _maxHealth;
        public int Health => _health;
        public int Level => _level;
        public int Coins => _coins;
    }
}