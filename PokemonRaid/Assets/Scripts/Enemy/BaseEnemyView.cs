using System;
using Pokemon.Animations;
using UnityEngine;

namespace Enemy
{
    public abstract class BaseEnemyView : MonoBehaviour
    {
        [SerializeField] private int _level;
        [SerializeField] private Animator _animator;
        [SerializeField] private LayerMask _pokemonLayer;
        [SerializeField] private AnimationEventTranslator _eventTranslator;

        public int Level => _level;
        public Transform Transform => transform;
        public Animator Animator => _animator;
        public LayerMask PokemonLayer => _pokemonLayer;
        public AnimationEventTranslator EventTranslator => _eventTranslator;

        public event Action<int> DamageTaken;
        public event Action ViewDestroyed;

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
    }
}