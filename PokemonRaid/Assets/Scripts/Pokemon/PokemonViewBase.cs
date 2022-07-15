using System;
using UnityEngine;

namespace Pokemon
{
    public class PokemonViewBase : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] protected LayerMask _enemyLayer;

        public Transform Transform => transform;
        public Animator Animator => _animator;
        public LayerMask EnemyLayer => _enemyLayer;

        public event Action<int> DamageTaken;
        public event Action ViewDestroyed;
        public event Func<int> LevelRequested;

        public void SetViewActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        
        public void TakeDamage(int damage)
        {
            DamageTaken?.Invoke(damage);
        }

        private void OnDestroy()
        {
            ViewDestroyed?.Invoke();
        }

        public int GetPokemonLevel()
        {
            if (LevelRequested == null)
            {
                return default;
            }
            
            var level = LevelRequested.Invoke();
            return level;

        }
    }
}