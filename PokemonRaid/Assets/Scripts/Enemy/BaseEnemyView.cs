using System;
using HealthBar;
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
        [SerializeField] private Collider _collider;
        [SerializeField] private ParticleSystem _meleeDamageParticle;
        [SerializeField] private ParticleSystem _rangeDamageParticle;
        [SerializeField] private HealthBarView _healthBarView;

        public int Level => _level;
        public Transform Transform => transform;
        public Animator Animator => _animator;
        public LayerMask PokemonLayer => _pokemonLayer;
        public AnimationEventTranslator EventTranslator => _eventTranslator;
        public ParticleSystem MeleeDamageParticle => _meleeDamageParticle;
        public ParticleSystem RangeDamageParticle => _rangeDamageParticle;
        public HealthBarView HealthBarView => _healthBarView;

        public event Action<int, PokemonType> DamageTaken;
        public event Action ViewDestroyed;

        public void SetViewActive(bool isActive)
        {
            //gameObject.SetActive(isActive);
            _collider.enabled = isActive;
        }
        
        public void SetHealth(float health)
        {
            _healthBarView.SetHealth(health);
        }

        public void TakeDamage(int damage, PokemonType pokemonType)
        {
            DamageTaken?.Invoke(damage, pokemonType);
        }

        private void OnDestroy()
        {
            ViewDestroyed?.Invoke();
        }
    }
}