using System;
using System.Collections.Generic;
using HealthBar;
using Pokemon.Animations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pokemon
{
    public class PokemonViewBase : MonoBehaviour
    {
        [SerializeField] protected LayerMask _enemyLayer;
        [SerializeField] protected LayerMask _boundsLayer;
        [SerializeField] private HealthBarView _healthBar;
        [SerializeField] private List<AnimationEventTranslator> _eventTranslators;
        [SerializeField] private List<Animator> _animators;
        [SerializeField] private Collider _collider;
        [SerializeField] private ParticleSystem _mergeParticle;
        [SerializeField] private ParticleSystem _spawnParticle;
        [SerializeField] private PokemonType _pokemonType;

        private int _level;

        public Transform Transform => transform;

        public Animator Animator => _animators.Count >= _level
            ? _animators[_level]
            : throw new ArgumentException("There is no animator for the level " + _level);
        public AnimationEventTranslator EventTranslator => _eventTranslators.Count >= _level
            ? _eventTranslators[_level]
            : throw new ArgumentException("There is no event translator for the level " + _level);
        public LayerMask EnemyLayer => _enemyLayer;
        public LayerMask BoundsLayer => _boundsLayer;

        public HealthBarView HealthBarView => _healthBar;
        public ParticleSystem MergeParticle => _mergeParticle;
        public ParticleSystem SpawnParticle => _spawnParticle;
        public PokemonType PokemonType => _pokemonType;

        public event Action<int> DamageTaken;
        public event Action ViewDestroyed;
        public event Action<int[]> IndexesSet;
        public event Func<int> LevelRequested;
        public event Func<int[]> IndexesRequested;

        public void SetLevel(int level)
        {
            _level = level - 1;
            
            foreach (var eventTranslator in _eventTranslators)
            {
                eventTranslator.gameObject.SetActive(false);
            }
            
            _eventTranslators[_level].gameObject.SetActive(true);
        }

        public void SetViewActive(bool isActive)
        {
            //gameObject.SetActive(isActive);
            _collider.enabled = isActive;
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