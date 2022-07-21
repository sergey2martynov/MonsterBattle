using System;
using HealthBar;
using UnityEngine;

namespace Pokemon
{
    public class PokemonViewBase : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] protected LayerMask _enemyLayer;
        [SerializeField] protected LayerMask _boundsLayer;
        [SerializeField] private HealthBarView _healthBar;

        public Transform Transform => transform;
        public Animator Animator => _animator;
        public LayerMask EnemyLayer => _enemyLayer;
        public LayerMask BoundsLayer => _boundsLayer;

        public HealthBarView HealthBarView => _healthBar;

        public event Action<int> DamageTaken;
        public event Action ViewDestroyed;
        public event Action<int[]> IndexesSet;
        public event Func<int> LevelRequested;
        public event Func<int[]> IndexesRequested;

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

        public void MakeActiveHealthBar()
        {
            _healthBar.gameObject.SetActive(true);
        }

        public void SetHealth(float health)
        {
            _healthBar.SetHealth(health);
        }

        public int[] GetIndexes()
        {
            return IndexesRequested?.Invoke();
        }

        public void SetIndexes(int[] newIndexes)
        {
            IndexesSet?.Invoke(newIndexes);
        }
    }
}