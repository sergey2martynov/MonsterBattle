using System;
using UnityEngine;

namespace Pokemon
{
    public class PokemonViewBase : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;

        public Transform Transform => transform;
        public Animator Animator => _animator;
        
        public event Action ViewDestroyed;
        public event Func<int> LevelRequested;

        public void SetViewActive(bool isActive)
        {
            gameObject.SetActive(isActive);
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