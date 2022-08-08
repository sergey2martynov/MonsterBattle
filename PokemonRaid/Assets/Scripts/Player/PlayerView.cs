using System;
using Arena;
using HealthBar;
using InputPlayer;
using UnityEngine;

namespace Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] protected HealthBarView _healthBarView;
        [SerializeField] private LayerMask _boundsLayer;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private InputView _inputView;
        [SerializeField] private ArenaMenuView _arenaMenuView;

        public Transform Transform => transform;
        public Animator Animator => _animator;
        public LayerMask BoundsLayer => _boundsLayer;
        public HealthBarView HealthBarView => _healthBarView;
        public ParticleSystem Particle => _particle;
        public InputView InputView => _inputView;
        public ArenaMenuView ArenaMenuView => _arenaMenuView;

        public event Action ViewDestroyed;
        public event Action LevelFinished;

        public void SetViewActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        
        public void SetHealth(float health)
        {
            _healthBarView.SetHealth(health);
        }

        public void LevelFinish()
        {
            LevelFinished?.Invoke();
        }

        private void OnDestroy()
        {
            ViewDestroyed?.Invoke();
        }
    }
}
